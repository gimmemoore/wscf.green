using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Thinktecture.Tools.Web.Services.ContractFirst;
using Thinktecture.Tools.Web.Services.CodeGeneration;


namespace Thinktecture.Tools.Wscf.UI.Dialogs
{
    public partial class WebServiceCodeGenDialogNew : Form
    {

        #region "private non-UI properties"
        private ArrayList wsdlFileCache;
        private bool externalFile = false;
		private string wsdlLocation = string.Empty;

        #endregion

        #region "Public Properties"
        public bool ServiceCode
        {
            get { return rbServer.Checked; }
        }

        public bool ClientCode
        {
            get { return rbClient.Checked; }
        }

        public string DestinationFilename
        {
            get { return tbDestinationFilename.Text; }
            set { tbDestinationFilename.Text = value; }
        }

        public string DestinationNamespace
        {
            get { return tbDestinationNamespace.Text; }
            set { tbDestinationNamespace.Text = value; }
        }

        public bool GenerateProperties
        {
            get { return cbProperties.Checked; }
        }

        public bool FormatSoapActions
        {
            get { return cbFormatSoapActions.Checked; }
        }

        public bool Collections
        {
            get { return cbCollections.Checked; }
        }

        public string WsdlLocation
        {
			get { return (cbWsdlLocation.SelectedItem != null) ? wsdlFileCache[cbWsdlLocation.SelectedIndex].ToString() : cbWsdlLocation.Text; }
			set { wsdlLocation = value; }
        }

        public bool ExternalFile
        {
            get { return externalFile; }
        }

        public bool GenerateMultipleFiles
        {
            get { return cbMultipleFiles.Checked; }
        }

        public bool Overwrite
        {
            get { return this.cbOverwrite.Checked; }
        }

        public bool ChangeCasing
        {
            get { return this.cbAdjustCasing.Checked; }
        }

        public bool EnableDataBinding
        {
            get { return this.cbDataBinding.Checked; }
        }

        public bool OrderIdentifiers
        {
            get { return this.cbOrderIds.Checked; }
        }

        public bool AsyncMethods
        {
            get { return this.cbAsync.Checked; }
        }

        public bool GenericList
        {
            get { return this.cbGenericList.Checked; }
        }

        public bool EnabledWsdlEndpoint
        {
            get { return this.cbEnableWsdlEndpoint.Checked; }
        }

        public string InstanceContextMode
        {
            get { return cbInstanceContextMode.Text; }
        }

        public string ConcurrencyMode
        {
            get { return cbConcurrencyMode.Text; }
        }

        public bool UseSynchronizationContext
        {
            get { return cbUseSynchronizationContext.Checked; }
        }

        public bool GenerateSvcFile
        {
            get { return cbGenerateSvcFile.Checked; }
        }

        public MethodImplementation MethodImplementation
        {
            get
            {
                if (rbNotImplementedException.Checked)
                {
                    return MethodImplementation.NotImplementedException;
                }
                if (rbPartialClassMethodCalls.Checked)
                {
                    return MethodImplementation.PartialClassMethodCalls;
                }
                if (rbAbstractMethods.Checked)
                {
                    return MethodImplementation.AbstractMethods;
                }
                return MethodImplementation.NotImplementedException;
            }
        }

        
        public CodeGenerationOptions Options
        {

            get { return this.options; }
        }


        #endregion



        private void SaveFormValues()
        {
            ConfigurationManager config = ConfigurationManager.GetConfigurationManager("WSCF05");
            config.Write("ClientCode", rbClient.Checked.ToString());
            config.Write("ServerCode", rbServer.Checked.ToString());

            config.Write("Properties", cbProperties.Checked.ToString());
            config.Write("Serializable", cbFormatSoapActions.Checked.ToString());
            config.Write("Collections", cbCollections.Checked.ToString());
            config.Write("GenericList", cbGenericList.Checked.ToString());
            config.Write("DataBinding", cbDataBinding.Checked.ToString());
            config.Write("OrderIdentifiers", cbOrderIds.Checked.ToString());
            config.Write("AsyncMethods", cbAsync.Checked.ToString());
            config.Write("MultipleFiles", cbMultipleFiles.Checked.ToString());
            config.Write("AdjustCasing", cbAdjustCasing.Checked.ToString());
            config.Write("ConcurrencyMode", cbConcurrencyMode.Text);
            config.Write("InstanceContextMode", cbInstanceContextMode.Text);
            config.Write("UseSynchronizationContext", cbUseSynchronizationContext.Checked.ToString());
            config.Write("EnableWsdlEndpoint", cbEnableWsdlEndpoint.Checked.ToString());
            config.Write("GenerateSvcFile", cbGenerateSvcFile.Checked.ToString());
            config.Write("MethodImplementation", MethodImplementation.ToString());

            config.Write("DestinationFilename", tbDestinationFilename.Text);
            config.Write("DestinationNamespace", tbDestinationNamespace.Text);

            config.Write("Overwrite", cbOverwrite.Checked.ToString());

            // BDS: Modified the code to store the values pasted to the combo box.
            if (cbWsdlLocation.SelectedItem != null)
            {
                config.Write("WSDLLocation", wsdlFileCache[cbWsdlLocation.SelectedIndex].ToString());
            }
            else
            {
                config.Write("WSDLLocation", cbWsdlLocation.Text);
            }

			config.Write("RememberSettings", cbSettings.Checked.ToString());


            string wsdlUrlsString = "";

            // Add the current item.
            if (cbWsdlLocation.SelectedItem == null)
            {
                string fname = AddWsdlFileToCache(cbWsdlLocation.Text);
            }

            foreach (string path in wsdlFileCache)
            {
                wsdlUrlsString += path + ";";
            }

            config.Write("WsdlUrls", wsdlUrlsString);
            config.Persist();
        }

        private void LoadFormValues()
        {
            ConfigurationManager config = ConfigurationManager.GetConfigurationManager("WSCF05");
            string wsdlUrls = config.Read("WsdlUrls");

            //if (wsdlUrls.Length > 0)
            //{
            cbWsdlLocation.Items.Clear();
            wsdlUrls = wsdlUrls.Trim(';');
            string[] urls = wsdlUrls.Split(';');

            // BDS: Changed this code to use new wsdl file cache.
            for (int urlIndex = 0; urlIndex < urls.Length; urlIndex++)
            {
                string fname = AddWsdlFileToCache(urls[urlIndex]);
            }

            if (cbWsdlLocation.Items.Count > 0)
            {
                cbWsdlLocation.SelectedIndex = 0;
            }

            if (wsdlLocation.Length > 0)
            {
				if (!wsdlFileCache.Contains(wsdlLocation))
                {
					string fname = AddWsdlFileToCache(wsdlLocation);
                }
                else
                {
					int wsdlIndex = wsdlFileCache.IndexOf(wsdlLocation);
                    cbWsdlLocation.SelectedIndex = wsdlIndex;
                }
            }
            //}

            if (config.ReadBoolean("RememberSettings"))
            {
                //In an MVP kind of approach, the config settings would be read into
                // an options class and that would be passed on to the view. 
                cbSettings.Checked = config.ReadBoolean("RememberSettings");
                rbClient.Checked = config.ReadBoolean("ClientCode");
                rbServer.Checked = config.ReadBoolean("ServerCode");

                cbProperties.Checked = config.ReadBoolean("Properties");
                cbFormatSoapActions.Checked = config.ReadBoolean("Serializable");
                cbCollections.Checked = config.ReadBoolean("Collections");
                cbGenericList.Checked = config.ReadBoolean("GenericList");
                cbDataBinding.Checked = config.ReadBoolean("DataBinding");
                cbOrderIds.Checked = config.ReadBoolean("OrderIdentifiers");
                cbAsync.Checked = config.ReadBoolean("AsyncMethods");
                cbMultipleFiles.Checked = config.ReadBoolean("MultipleFiles");
                cbAdjustCasing.Checked = config.ReadBoolean("AdjustCasing");
				cbConcurrencyMode.SelectedItem = config.Read("ConcurrencyMode", "Single");
				cbInstanceContextMode.SelectedItem = config.Read("InstanceContextMode", "PerCall");
                cbUseSynchronizationContext.Checked = config.ReadBoolean("UseSynchronizationContext");
                cbEnableWsdlEndpoint.Checked = config.ReadBoolean("EnableWsdlEndpoint");
                cbGenerateSvcFile.Checked = config.ReadBoolean("GenerateSvcFile");
                string methodImplementationValue = config.Read("MethodImplementation", MethodImplementation.NotImplementedException.ToString());
                MethodImplementation methodImplementation = (MethodImplementation)Enum.Parse(typeof(MethodImplementation), methodImplementationValue);
                if (methodImplementation == MethodImplementation.NotImplementedException)
                {
                    rbNotImplementedException.Checked = true;
                }
                if (methodImplementation == MethodImplementation.PartialClassMethodCalls)
                {
                    rbPartialClassMethodCalls.Checked = true;
                }
                if (methodImplementation == MethodImplementation.AbstractMethods)
                {
                    rbAbstractMethods.Checked = true;
                }

                tbDestinationFilename.Text = config.Read("DestinationFilename");
                tbDestinationNamespace.Text = config.Read("DestinationNamespace");

                cbOverwrite.Checked = config.ReadBoolean("Overwrite");
            }
        }

        /// <summary>
        /// Adds a wsdl file info to the wsdl file cache.
        /// </summary>
        /// <param name="path">Path of the wsdl file to add.</param>
        /// <returns>A string indicating the name of the wsdl file.</returns>
        /// <author>BDS - thinktecture</author>
        private string AddWsdlFileToCache(string path)
        {
            if (path.LastIndexOf("\\") > 0 && path.ToLower().EndsWith(".wsdl"))
            {
                if (wsdlFileCache.Count == 10)
                {
                    wsdlFileCache.RemoveAt(0);
                    cbWsdlLocation.Items.RemoveAt(0);
                }
                string fname = path.Substring(path.LastIndexOf("\\") + 1);
                wsdlFileCache.Add(path);
                cbWsdlLocation.SelectedIndex = cbWsdlLocation.Items.Add(fname);
                return fname;
            }

            return "";
        }

        private string GetFileNameFromPath(string path)
        {
            string fname = "";
            if (path.LastIndexOf("\\") < path.Length - 1)
            {
                fname = path.Substring(path.LastIndexOf("\\") + 1);
            }
            return fname;
        }

        private void HandleClientOrServerGeneration()
        {
            bool isValid = true;
            isValid = ValidateWsdlLocation();
            isValid = ValidateDestinationNamespace();
            isValid = ValidateDestinationFileName();
            if(!isValid)
            {
                ForceNoResult();
            }
            else
            {
                SetOptions();
                CompleteProcessing();
            }
        }

        private void SetOptions()
        {
            //General options
			options.MetadataLocation = this.WsdlLocation;
            options.OutputFileName = this.DestinationFilename;
            options.OverwriteExistingFiles = this.Overwrite;
            options.GenerateSeparateFiles = this.GenerateMultipleFiles;

            //Data Contract options
            options.ClrNamespace = this.DestinationNamespace;
            options.GenerateProperties = this.GenerateProperties;
            options.GenerateCollections = this.Collections;
            options.GenerateTypedLists = this.GenericList;
            options.EnableDataBinding = this.EnableDataBinding;
            options.GenerateOrderIdentifiers = this.OrderIdentifiers;
            options.GenerateAsyncCode = this.AsyncMethods;
            options.AdjustCasing = this.ChangeCasing;

            //Options for the service stub            
            options.GenerateService = this.ServiceCode;
            options.EnableWsdlEndpoint = this.EnabledWsdlEndpoint;
            options.GenerateSvcFile = this.GenerateSvcFile;

            //options for the service contract
            options.FormatSoapActions = this.FormatSoapActions;
            
            //Options for the service lifetime etc
            options.ConcurrencyMode = this.ConcurrencyMode;
            options.InstanceContextMode = this.InstanceContextMode;
            options.UseSynchronizationContext = this.UseSynchronizationContext;

        }

        private void CompleteProcessing()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ForceNoResult()
        {
            this.DialogResult = DialogResult.None;
            btnGenerate.DialogResult = DialogResult.None;
        }
    }
}
