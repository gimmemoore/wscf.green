using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using Thinktecture.Tools.Wscf.UI.Dialogs;
using Thinktecture.Tools.Web.Services.Wscf.Environment;
using Thinktecture.Tools.Web.Services.CodeGeneration;
using Thinktecture.Tools.Wscf.UI.WsdlWizard;

namespace Thinktecture.Tools.Web.Services.ContractFirst
{
    internal class ServiceFacade
    {
        private readonly VisualStudio vsInstance;
        private readonly VisualStudioSelectedItem selectedItem;
        private readonly VisualStudioProject currentProject;
		private readonly OutputWindowWriter outputWindowWriter;
        
        public ServiceFacade(VisualStudio visualStudio)
        {
            vsInstance = visualStudio;
            selectedItem = vsInstance.SelectedItem;
            currentProject = vsInstance.SelectedProject;
			outputWindowWriter = new OutputWindowWriter(visualStudio.ApplicationObject);
        }
        
        public bool ProcessServiceRequest(string commandName)
        {
			if (commandName == MenuBuilder.MenuWscfContextMenu2 ||
				commandName == MenuBuilder.MenuWscf ||
				commandName == MenuBuilder.MenuWscfContextMenu)
            {
                return ProcessStubGenerationRequest();
            }
			if (commandName == MenuBuilder.MenuCreateWsdl)
        	{
        		return ProcessWsdlWizardRequest(false);
        	}
			if (commandName == MenuBuilder.MenuEditWsdl)
        	{
        		return ProcessWsdlWizardRequest(true);
        	}
			if (commandName == MenuBuilder.MenuGenerateDataContracts)
        	{
        		return ProcessXsdCodeGenerationRequest();
        	}
			if (commandName == MenuBuilder.MenuPasteXmlAsSchema)
			{
				return ProcessPasteSchemaRequest();
			}

            return true;
            
        }
        
        
        private bool ProcessWsdlWizardRequest(bool roundtrip)
        {
            if(!ValidateItemAndProject()) return false;
            
            string currentDir = selectedItem.Directory;
            string projectRootDir = selectedItem.ParentProject.ProjectDirectory;
            string metdataFile = selectedItem.FileName;

            WsdlWizardForm wizard = null;

            try
            {
                wizard = (roundtrip) ? new WsdlWizardForm(metdataFile, true) : new WsdlWizardForm(metdataFile);
                
                wizard.WsdlLocation = currentDir;
                wizard.DefaultPathForImports = "";
                wizard.ProjectRootDirectory = projectRootDir;
                wizard.ShowDialog();

            	if (wizard.DialogResult == DialogResult.OK)
                {
                    if (wizard.WsdlLocation.Length > 0)
                    {
                        string wsdlFile = wizard.WsdlLocation;

                        if (!roundtrip)
                        {
                            currentProject.AddFile(wsdlFile);
                        }

                        if (wizard.OpenCodeGenDialog)
                        {
                            ProcessCodeGenerationRequest(wsdlFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "WSDL Wizard", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            	if (wizard != null) wizard.Close();
            	return false;
            }

            return true;
        }

        private bool ProcessXsdCodeGenerationRequest()
        {
			try
			{
				VisualStudioProject project = currentProject;
				IEnumerable<VisualStudioSelectedItem> selectedItems = vsInstance.SelectedItems;

				if (selectedItems.Count() == 0)
				{
					MessageBox.Show(
						"Cannot generate code for items outside of a project.",
						"Web Services Contract-First code generation",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				foreach (VisualStudioSelectedItem selectedItem in selectedItems)
				{
					string extension = Path.GetExtension(selectedItem.FileName).ToLower();
					if (extension == ".xsd" || extension == ".wsdl") continue;

					MessageBox.Show(
						"Data Contracts can only be generated for .xsd or .wsdl files.",
						"Web Services Contract-First code generation",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				string[] dataContractFiles = selectedItems.Select(i => i.FileName).ToArray();
				XsdCodeGenDialog dialogForm = new XsdCodeGenDialog(dataContractFiles);
				if (!project.IsWebProject)
				{
					dialogForm.Namespace = project.AssemblyNamespace;
				}
				dialogForm.TargetFileName = project.GetDefaultDestinationFilename(dataContractFiles[0]);

				if (dialogForm.ShowDialog() == DialogResult.Cancel)
				{
					return false;
				}

				CodeGenerationOptions options = new CodeGenerationOptions();
				options.GenerateDataContracts = true;
				options.DataContractFiles = dataContractFiles;
				options.GenerateProperties = dialogForm.PublicProperties;
				options.GenerateCollections = dialogForm.Collections;
				options.GenerateSeparateFiles = dialogForm.GenerateMultipleFiles;
				options.OverwriteExistingFiles = dialogForm.OverwriteFiles;
				options.AdjustCasing = dialogForm.AdjustCasing;
				options.EnableDataBinding = dialogForm.DataBinding;
				options.GenerateOrderIdentifiers = dialogForm.OrderIdentifiers;
				options.GenerateTypedLists = dialogForm.GenericLists;
				options.ClrNamespace = dialogForm.Namespace;
				options.OutputFileName = dialogForm.TargetFileName;
				options.OutputLocation = GetOutputDirectory();
				options.ProjectDirectory = project.ProjectDirectory;
				options.Language = project.ProjectLanguage;

				CodeGenerator codeGenerator = new CodeGenerator();
				CodeWriterOutput output = codeGenerator.GenerateCode(options);

				AddGeneratedFilesToProject(output);

				// Finally add the project references.
				AddAssemblyReferences();

				MessageBox.Show("Code generation successfully completed.", "WSCF.Blue", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				AppLog.LogMessage(ex.ToString());
				MessageBox.Show(ex.ToString(), "CodeGeneration", MessageBoxButtons.OK, MessageBoxIcon.Error);
				// TODO: Log the exception.
				//System.Diagnostics.Debugger.Break();
			}

            return true;
        } // End of ProcessXsdCodeGenerationRequestFunction.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wsdlFile"></param>
        /// <returns></returns>
        public bool ProcessCodeGenerationRequest(string wsdlFile)
        {
            if (!File.Exists(wsdlFile))
            {
                return false;
            }
            if (!IsLocatedInProject())
            {
                return false;
            }
            return ProcessStubGenerationRequest(wsdlFile);
        }

        /// <summary>
        /// This method handles the service that generates stubs for the client side or the service side.
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ProcessStubGenerationRequest()
        {
            string wsdlFile = String.Empty;
            
            if (!IsLocatedInProject())
            {
                return false;
            }

            // if there is a selected item, then set that as the wsdl file, otherwise
            // leave it empty and the user will make use of the BROWSE button to get the
            // wsdl file name

            if (!selectedItem.IsProject)
            {
                wsdlFile = selectedItem.FileName;
            }

            return ProcessStubGenerationRequest(wsdlFile);

        }

        /// <summary>
        /// This overloaded method handles the generation of client or server code
        /// for a supplied WSDL file
        /// </summary>
        /// <param name="wsdlFile"></param>
        /// <returns></returns>
        private bool ProcessStubGenerationRequest(string wsdlFile)
        {
            try
            {
                // Fist display the UI and get the options.
                WebServiceCodeGenDialogNew dialog = new WebServiceCodeGenDialogNew();
                if (!currentProject.IsWebProject)
                {
                    dialog.DestinationNamespace = currentProject.AssemblyNamespace;
                }
                dialog.DestinationFilename = GetDefaultDestinationFilename(wsdlFile);

                if (!selectedItem.IsProject)
                {
                    //dialog.WsdlLocation = selectedItem.FileName;
                    dialog.WsdlLocation = wsdlFile;
                }

                //if we havent passed the wsdl file in, there is still 
                // the option for the user to use the BROWSE button and pick
                // the wsdl. 

                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return false;
                }


                //Get the options set in the dialog and add other things
                // that are specific to VS 
                CodeGenerationOptions options = dialog.Options;

				options.Language = currentProject.ProjectLanguage;
                options.ProjectDirectory = currentProject.ProjectDirectory;
                options.OutputLocation = GetOutputDirectory();
                // TODO: Infer the config file type according to the project type
                // and merge the generated config file with the existing one.
                options.ConfigurationFile = "output.config";

				outputWindowWriter.Clear();

                CodeGenerator codeGenerator = new CodeGenerator();
                CodeWriterOutput output = codeGenerator.GenerateCode(options);

                AddGeneratedFilesToProject(output);

                // Finally add the project references.
                AddAssemblyReferences();

				// add custom assembly references if necessary
				if (options.EnableWsdlEndpoint)
				{
					AddMetadataExtensionsReference();
				}

                MessageBox.Show("Code generation successfully completed.", "WSCF.Blue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
			catch (ClientServiceGenerationException ex)
			{
				AppLog.LogMessage(ex.ToString());

				const string separator = "---------------------------------------------------------------------------------";
				foreach (string message in ex.Messages)
				{
					outputWindowWriter.WriteMessage(message + "\r\n" + separator + "\r\n");
				}

				MessageBox.Show("Errors were found while importing the contract. Please check the 'WSCF.blue' pane in the Output window for more information.",
					"CodeGeneration", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
            catch (Exception ex)
            {
                AppLog.LogMessage(ex.ToString());
                MessageBox.Show(ex.ToString(), "CodeGeneration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // TODO: Log the exception.
                //System.Diagnostics.Debugger.Break();
                return false;
            }
            return true;

        }

		private bool ProcessPasteSchemaRequest()
		{
			try
			{
				if (currentProject == null)
				{
					MessageBox.Show(
						"You cannot paste schema items outside of a project.",
						"Web Services Contract-First Paste Schema",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject == null)
				{
					MessageBox.Show(
						"There was no data found in the clipboard.",
						"Web Services Contract-First Paste Schema",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				string xml = (string)dataObject.GetData(typeof(string));
				if (xml == null)
				{
					MessageBox.Show(
						"There was no string data found in the clipboard.",
						"Web Services Contract-First Paste Schema",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				XElement schemaXml;
				try
				{
					schemaXml = XElement.Parse(xml);
				}
				catch (Exception)
				{
					MessageBox.Show(
						"The data found in the clipboard is not valid XML.",
						"Web Services Contract-First Paste Schema",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return true;
				}

				XmlSchemaSet schemas = new XmlSchemaSet();
				XmlSchemaInference inference = new XmlSchemaInference();

				using (XmlReader reader = schemaXml.CreateReader())
				{
					inference.InferSchema(reader, schemas);
				}

				XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

				foreach (XmlSchema schema in schemas.Schemas())
				{
					if (schema.Items.Count == 0) continue;

					string schemaName = ((XmlSchemaElement)(schema.Items[0])).Name;
					string fileName = Path.Combine(selectedItem.Directory, Path.ChangeExtension(schemaName, "xsd"));

					if (File.Exists(fileName))
					{
						DialogResult dialogResult = MessageBox.Show(
							"A file named '" + Path.GetFileName(fileName) + "' already exist in the project. Do you want to overwrite this file?",
							"Web Services Contract-First Paste Schema",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question);

						if (dialogResult == DialogResult.No) continue;
					}

					using (XmlWriter writer = XmlWriter.Create(fileName, settings))
					{
						if (writer != null)
						{
							schema.Write(writer);
						}
					}
					if (File.Exists(fileName))
					{
						currentProject.AddFile(fileName);
					}
				}
			}
			catch (Exception ex)
			{
				AppLog.LogMessage(ex.ToString());
				MessageBox.Show(ex.ToString(), "Web Services Contract-First Paste Schema", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

        private void AddGeneratedFilesToProject(CodeWriterOutput output)
        {
            foreach (string file in output.CodeFileNames)
            {
                currentProject.AddFile(file);
            }
			if (!string.IsNullOrEmpty(output.ConfigurationFile) && File.Exists(output.ConfigurationFile))
			{
				currentProject.AddFile(output.ConfigurationFile);				
			}
        }
        
        private bool IsLocatedInProject()
        {
            // Do we have a project?
            if (currentProject == null)
            {
                MessageBox.Show("Cannot generate code for items outside of a project.", "WSCF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private string GetOutputDirectory()
        {
            //TODO: May need to be enhanced to handle different types of templates
            //VisualStudioProject project = selectedItem.ParentProject;
            string outputDirectory;

            if (currentProject.IsWebProject)
            {
                string serviceName = Path.GetFileNameWithoutExtension(selectedItem.FileName);
                outputDirectory = currentProject.AddCodeFolderToWebProject(serviceName);
            }
            else
            {
                outputDirectory = selectedItem.Directory;
            }

            return outputDirectory;
        }


        private void AddAssemblyReferences()
        {
            //TODO: we need to find a way to extend this to add the 
            //metadata extensions if Enable WSDL is selected.
            currentProject.AddReference("System");
            currentProject.AddReference("System.Xml");
            currentProject.AddReference("System.Runtime.Serialization");
            currentProject.AddReference("System.ServiceModel");
            currentProject.AddReference("System.Configuration");
        }

		/// <summary>
		/// Add more assembly references beyond the basic Fx assemblies
		/// </summary>
		private void AddMetadataExtensionsReference()
		{
			string addinDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string extensionPath = Path.Combine(addinDirectory, "Thinktecture.ServiceModel.Extensions.Metadata.dll");

			currentProject.AddReference(extensionPath);
		}

        private static string GetDefaultDestinationFilename(string wsdlFile)
        {
            return Path.GetFileNameWithoutExtension(wsdlFile);
        }

        private bool ValidateItemAndProject()
        {


            if (selectedItem == null)
            {
                MessageBox.Show("No selected item.",
                        "Web Services Contract-First WSDL Wizard", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return false;
            }

            if (!selectedItem.HasProject)
            {
                MessageBox.Show("Cannot create a WSDL contract for items outside of a project.",
                    "Web Services Contract-First WSDL Wizard", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return false;
            }

            return true;

        }


    }
}
