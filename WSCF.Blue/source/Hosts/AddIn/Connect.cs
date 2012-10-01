using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using EnvDTE;
using Extensibility;
using VSLangProj;
using VSLangProj80;
using VsWebSite;
using System.Diagnostics;
using Microsoft.VisualStudio.CommandBars;
using System.Text;
using Microsoft.Win32;
using EnvDTE80;
using Thinktecture.Tools.Web.Services.Wscf.Environment;
using Thinktecture.Tools.Web.Services.CodeGeneration;
using Thinktecture.Tools.Wscf.UI.WsdlWizard;

namespace Thinktecture.Tools.Web.Services.ContractFirst
{
    using PrjKind = VSLangProj.PrjKind;
    using DteConstants = Constants;

    #region Read me for Add-in installation and setup information.
    // When run, the Add-in wizard prepared the registry for the Add-in.
    // At a later time, if the Add-in becomes unavailable for reasons such as:
    //   1) You moved this project to a computer other than which is was originally created on.
    //   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
    //   3) Registry corruption.
    // you will need to re-register the Add-in by building the WsContractFirstSetup project 
    // by right clicking the project in the Solution Explorer, then choosing install.
    #endregion

    /// <summary>
    ///   The object for implementing an Add-in.
    /// </summary>
    /// <seealso class='IDTExtensibility2' />
    //[ComVisible(true)]
    //[StructLayout(LayoutKind.Sequential)]
    //
    // NOTE: Enable comments starting with *** to enable the simple logging mechanism we use.
    //
    [Guid("6C6DA7B1-0758-41f4-9669-04ED4B037669"), ProgId("Thinktecture.Tools.Web.Services.ContractFirst.Connect")]
    public class Connect : Object, IDTExtensibility2, IDTCommandTarget
    {
        // BDS: Save the last file name and the file counter in a hash table.
        private Hashtable fileCounters = null;
        //private bool addDuplictedTypesAlert = false;
        //private bool overwriteFiles = false;
        private DTE2 applicationObject;
        private AddIn addInInstance;
        private VisualStudio visualStudio;
    	private MenuBuilder menuBuilder;

        /// <summary>
        ///		Implements the constructor for the Add-in object.
        ///		Place your initialization code within this method.
        /// </summary>
        public Connect()
        {
            if (fileCounters == null)
            {
                fileCounters = new Hashtable();
            }
            AppLog.LogMessage("A new instance of WSCF Connect class is created.");
        }

        /// <summary>
        ///      Implements the OnConnection method of the IDTExtensibility2 interface.
        ///      Receives notification that the Add-in is being loaded.
        /// </summary>
        /// <param term='application'>
        ///      Root object of the host application.
        /// </param>
        /// <param term='connectMode'>
        ///      Describes how the Add-in is being loaded.
        /// </param>
        /// <param term='addInInst'>
        ///      Object representing this Add-in.
        /// </param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            AppLog.LogMessage("Entering OnConnection method.");

            this.applicationObject = (DTE2)application;
            // Initialize VsHelper.
            this.visualStudio = new VisualStudio(this.applicationObject);

            this.addInInstance = (AddIn)addInInst;
            object[] contextGUIDS = new object[] { };
            //CommandBar cmdBar;
            //Command cmdObj;

            if (connectMode == ext_ConnectMode.ext_cm_Startup ||
                connectMode == ext_ConnectMode.ext_cm_AfterStartup ||
                connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                AppLog.LogMessage("Creating commands.");

                Commands2 commands = (Commands2)applicationObject.Commands;
                CommandBars commandBars = (CommandBars)applicationObject.CommandBars;

                try
                {
					menuBuilder = new MenuBuilder(visualStudio);

                    // Add command bar to tools menu
					contextGUIDS = menuBuilder.AddCommandBarToToolsMenu(addInInstance, applicationObject, commands, contextGUIDS, commandBars);
                    // Create the 'Generate Web Service Code...' context-menu entry
					contextGUIDS = menuBuilder.GenerateMenuItemForWSCodeGen(addInInstance, applicationObject, commands, contextGUIDS);
                    // Create the 'Edit WSDL Interface Description...' context-menu entry
					contextGUIDS = menuBuilder.GenerateMenuItemForEditWsdl(addInInstance, applicationObject, commands, contextGUIDS);
                    // Create the 'Create WSDL Interface Description...' context-menu entry
					contextGUIDS = menuBuilder.GenerateMenuItemForCreateWsdl(addInInstance, applicationObject, commands, contextGUIDS);
                    // Create the 'Choose WSDL to implement...' context-menu entry
					contextGUIDS = menuBuilder.GenerateMenuItemForChooseWsdlToImplement(addInInstance, applicationObject, commands, contextGUIDS);
                    // Create the 'Generate code...' context-menu entry
                    //GenerateMenuItemForDCCodeGen(commands, contextGUIDS);
					contextGUIDS = menuBuilder.GenerateMenuItemForDCCodeGen(addInInstance, applicationObject, commands, contextGUIDS);
					// Create the 'Paste XML as Schema' Edit menu entry.
					menuBuilder.GenerateMenuItemForPasteXmlAsSchema(addInInstance, applicationObject, commands, commandBars, contextGUIDS);
                }
                catch (ArgumentException e)
                {
                    AppLog.LogMessage(e.Message);
                }
                catch (Exception e)
                {
                    AppLog.LogMessage(e.Message);
                }


            }

            AppLog.LogMessage("Leaving OnConnection method.");
        }

        /// <summary>
        ///     Implements the OnDisconnection method of the IDTExtensibility2 interface.
        ///     Receives notification that the Add-in is being unloaded.
        /// </summary>
        /// <param term='disconnectMode'>
        ///      Describes how the Add-in is being unloaded.
        /// </param>
        /// <param term='custom'>
        ///      Array of parameters that are host application specific.
        /// </param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            AppLog.LogMessage("Entering OnDisconnection method.");

            try
            {
                if (disconnectMode == ext_DisconnectMode.ext_dm_HostShutdown |
                    disconnectMode == ext_DisconnectMode.ext_dm_UserClosed)
                {
                	menuBuilder.CleanUp();
                }

            }
            catch (Exception ex)
            {
                AppLog.LogMessage(ex.Message);
                throw;
            }

            AppLog.LogMessage("Leaving OnDisconnection method.");
        }
        
        /// <summary>
        ///      Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
        ///      Receives notification that the collection of Add-ins has changed.
        /// </summary>
        /// <param term='custom'>
        ///      Array of parameters that are host application specific.
        /// </param>
        /// <seealso class='IDTExtensibility2' />
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>
        ///      Implements the OnStartupComplete method of the IDTExtensibility2 interface.
        ///      Receives notification that the host application has completed loading.
        /// </summary>
        /// <param term='custom'>
        ///      Array of parameters that are host application specific.
        /// </param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>
        ///      Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
        ///      Receives notification that the host application is being unloaded.
        /// </summary>
        /// <param term='custom'>
        ///      Array of parameters that are host application specific.
        /// </param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>
        ///      Implements the QueryStatus method of the IDTCommandTarget interface.
        ///      This is called when the command's availability is updated
        /// </summary>
        /// <param term='commandName'>
        ///		The name of the command to determine state for.
        /// </param>
        /// <param term='neededText'>
        ///		Text that is needed for the command.
        /// </param>
        /// <param term='status'>
        ///		The state of the command in the user interface.
        /// </param>
        /// <param term='commandText'>
        ///		Text requested by the neededText parameter.
        /// </param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            AppLog.LogMessage("Entering QueryStatus method.");

            try
            {
                status = vsCommandStatus.vsCommandStatusInvisible;
                if (commandName == MenuBuilder.MenuEditWsdl)
                {
                    status = QueryStatusForEditWsdl();
                }

				if (commandName == MenuBuilder.MenuWscfContextMenu)
                {
                    status = QueryStatusForContextMenu();
                }

				if (commandName == MenuBuilder.MenuCreateWsdl)
                {
                    status = QueryStatusForCreateWsdl();
                }

				if (commandName == MenuBuilder.MenuGenerateDataContracts)
                {
                    status = QueryStatusForGenDC();
                }

                if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
                {
					if (commandName == MenuBuilder.MenuWscfContextMenu2)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusEnabled |
                            vsCommandStatus.vsCommandStatusSupported;
                        AppLog.LogMessage("WSCF menu 2 command is enabled");
                    }
					if (commandName == MenuBuilder.MenuWscf)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
                            vsCommandStatus.vsCommandStatusEnabled;
                        AppLog.LogMessage("Contract first command is enabled");
                    }
					if (commandName == MenuBuilder.MenuPasteXmlAsSchema)
					{
						status = vsCommandStatus.vsCommandStatusSupported;
						if (visualStudio.SelectedProject != null)
						{
							status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
							AppLog.LogMessage("Paste Schema command is enabled");
						}
					}
                }
            }
            catch (Exception ex)
            {
                AppLog.LogMessage(ex.Message);
                throw;
            }

            AppLog.LogMessage("Leaving QueryStatus method.");
        }

        private vsCommandStatus QueryStatusForGenDC()
        {
            vsCommandStatus status = vsCommandStatus.vsCommandStatusInvisible;
            
            UIHierarchy uiHierarchy = (UIHierarchy)applicationObject.Windows.Item(
                                                       DteConstants.vsWindowKindSolutionExplorer).Object;
            foreach (UIHierarchyItem item in (Array)uiHierarchy.SelectedItems)
            {
                string itemName = item.Name;

                if (itemName.IndexOf(".xsd") > -1 || itemName.IndexOf(".wsdl") > -1)
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusEnabled |
                             vsCommandStatus.vsCommandStatusSupported;
                    AppLog.LogMessage("Generate code command is enabled");
                    break;
                }
            }
            return status;
        }

        private vsCommandStatus QueryStatusForCreateWsdl()
        {
            vsCommandStatus status = vsCommandStatus.vsCommandStatusInvisible;
            UIHierarchy uiHierarchy = (UIHierarchy)applicationObject.Windows.Item(
                                                       DteConstants.vsWindowKindSolutionExplorer).Object;
            foreach (UIHierarchyItem item in (Array)uiHierarchy.SelectedItems)
            {
                string itemName = item.Name;

                if (itemName.IndexOf(".xsd") > -1)
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusEnabled |
                             vsCommandStatus.vsCommandStatusSupported;
                    AppLog.LogMessage("Create WSDL command is enabled");
                    break;
                }
            }
            return status;
        }

        private vsCommandStatus QueryStatusForContextMenu()
        {
            vsCommandStatus status = vsCommandStatus.vsCommandStatusInvisible;
            UIHierarchy uiHierarchy = (UIHierarchy)applicationObject.Windows.Item(
                                                       DteConstants.vsWindowKindSolutionExplorer).Object;
            foreach (UIHierarchyItem item in (Array)uiHierarchy.SelectedItems)
            {
                string itemName = item.Name;

                if (itemName.IndexOf(".wsdl") > -1)
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusEnabled |
                             vsCommandStatus.vsCommandStatusSupported;
                    AppLog.LogMessage("WSCF context menu is enabled.");
                    break;
                }
            }
            return status;
        }

        private vsCommandStatus QueryStatusForEditWsdl()
        {
            vsCommandStatus status = vsCommandStatus.vsCommandStatusInvisible;
            UIHierarchy uiHierarchy = (UIHierarchy)applicationObject.Windows.Item(
                                                       DteConstants.vsWindowKindSolutionExplorer).Object;
            foreach (UIHierarchyItem item in (Array)uiHierarchy.SelectedItems)
            {
                string itemName = item.Name;

                if (itemName.IndexOf(".wsdl") > -1)
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusEnabled |
                             vsCommandStatus.vsCommandStatusSupported;
                    AppLog.LogMessage("Edit WSDL command is enabled.");
                    break;
                }
            }
            return status;
        }

        /// <summary>
        ///      Implements the Exec method of the IDTCommandTarget interface.
        ///      This is called when the command is invoked.
        /// </summary>
        /// <param term='commandName'>
        ///		The name of the command to execute.
        /// </param>
        /// <param term='executeOption'>
        ///		Describes how the command should be run.
        /// </param>
        /// <param term='varIn'>
        ///		Parameters passed from the caller to the command handler.
        /// </param>
        /// <param term='varOut'>
        ///		Parameters passed from the command handler to the caller.
        /// </param>
        /// <param term='handled'>
        ///		Informs the caller if the command was handled or not.
        /// </param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            AppLog.LogMessage("Entering Exec method.");
            ServiceFacade facade = new ServiceFacade(this.visualStudio);

            handled = facade.ProcessServiceRequest(commandName);

            AppLog.LogMessage("Leaving Exec method");

            return;



            
        }

        #region Private helper methods

               private void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Web Services Contract-First code generation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
 

        #endregion
    }
}
