using System;

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

using Thinktecture.Tools.Web.Services.Wscf.Environment;

namespace Thinktecture.Tools.Web.Services.ContractFirst
{
    internal class MenuBuilder
    {
    	private readonly VisualStudio visualStudio;
		private readonly SelectionEvents selectionEvents;
		private readonly CommandBar subMenuCommandBar;
    	private readonly CommandBar webSubMenuCommandBar;
    	private readonly CommandBar projectSubMenuCommandBar;
		private readonly CommandBar webProjectSubMenuCommandBar;
		private readonly CommandBar toolsSubMenuCommandBar;
		private readonly CommandBarPopup subMenuPopup;
		private readonly CommandBarPopup webSubMenuPopup;
		private readonly CommandBarPopup projectSubMenuPopup;
		private readonly CommandBarPopup webProjectSubMenuPopup;
		private readonly CommandBarPopup toolsSubMenuPopup;

    	public static readonly string MenuEditWsdl = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.EditWsdlContextMenu";
    	public static readonly string MenuWscfContextMenu = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.WsContractFirstContextMenu";
    	public static readonly string MenuCreateWsdl = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.CreateWsdlContextMenu";
    	public static readonly string MenuGenerateDataContracts = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.GenerateCodeMenu";
    	public static readonly string MenuWscfContextMenu2 = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.WsContractFirstContextMenu2";
    	public static readonly string MenuWscf = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.WsContractFirst";
    	public static readonly string MenuPasteXmlAsSchema = "Thinktecture.Tools.Web.Services.ContractFirst.Connect.PasteSchemaMenu";

    	public MenuBuilder(VisualStudio visualStudio)
    	{
    		this.visualStudio = visualStudio;

    		try
    		{
    			CommandBars commandBars = (CommandBars)visualStudio.ApplicationObject.CommandBars;
    			CommandBar itemCommandBar = commandBars["Item"];
    			CommandBar webItemCommandBar = commandBars["Web Item"];
				CommandBar projectCommandBar = commandBars["Project"];
				CommandBar webProjectCommandBar = commandBars["Web Project Folder"];
				CommandBar toolsCommandBar = commandBars["Tools"];

    			subMenuPopup = (CommandBarPopup)itemCommandBar.Controls.Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, 1, true);
    			subMenuPopup.Caption = "WSCF.blue";
    			subMenuCommandBar = subMenuPopup.CommandBar;

    			webSubMenuPopup = (CommandBarPopup)webItemCommandBar.Controls.Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, 1, true);
    			webSubMenuPopup.Caption = "WSCF.blue";
    			webSubMenuCommandBar = webSubMenuPopup.CommandBar;

				projectSubMenuPopup = (CommandBarPopup)projectCommandBar.Controls.Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, 1, true);
				projectSubMenuPopup.Caption = "WSCF.blue";
				projectSubMenuCommandBar = projectSubMenuPopup.CommandBar;

				webProjectSubMenuPopup = (CommandBarPopup)webProjectCommandBar.Controls.Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, 1, true);
				webProjectSubMenuPopup.Caption = "WSCF.blue";
				webProjectSubMenuCommandBar = webProjectSubMenuPopup.CommandBar;

				toolsSubMenuPopup = (CommandBarPopup)toolsCommandBar.Controls.Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, 1, true);
				toolsSubMenuPopup.Caption = "WSCF.blue";
				toolsSubMenuCommandBar = toolsSubMenuPopup.CommandBar;

    			selectionEvents = visualStudio.ApplicationObject.Events.SelectionEvents;
    			selectionEvents.OnChange += OnSelectionChanged;
    		}
    		catch (Exception ex)
    		{
    			AppLog.LogMessage(ex.ToString());
    		}			
    	}

    	public object[] AddCommandBarToToolsMenu(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS, CommandBars commandBars)
        {
            Command command = commands.AddNamedCommand2(
                addInInstance,
                "WsContractFirst",
                "Web Services Contract-First...",
                "Executes the command for WsContractFirstAddin",
                true,
                190,
                ref contextGUIDS,
                (int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText,
                vsCommandControlType.vsCommandControlTypeButton);

			command.AddControl(toolsSubMenuCommandBar, 1);

            AppLog.LogMessage("Command bar is added to the Tools menu.");
            return contextGUIDS;
        }

        public object[] GenerateMenuItemForWSCodeGen(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS)
        {
            Command cmdObj = commands.AddNamedCommand2(
            	addInInstance,
            	"WsContractFirstContextMenu",
            	"Generate Web Service Code...",
            	"Executes the command for WsContractFirstAddin ContextMenu",
            	true,
            	190,
            	ref contextGUIDS,
            	(int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
            	(int)vsCommandStyle.vsCommandStylePictAndText,
            	vsCommandControlType.vsCommandControlTypeButton);

			cmdObj.AddControl(subMenuCommandBar, 1);

            // BDS 11/21/2005: Add this menu item to the web project 
            // template.
			cmdObj.AddControl(webSubMenuCommandBar, 1);

            AppLog.LogMessage("Generate Web Serive Code menu item is added.");
            return contextGUIDS;
        }


        public object[] GenerateMenuItemForCreateWsdl(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS)
        {
            Command cmdObj = commands.AddNamedCommand2(
            	addInInstance,
            	"CreateWsdlContextMenu",
            	"Create WSDL Interface Description...",
            	"Executes the command for WsContractFirstAddin ContextMenu",
            	true,
            	190,
            	ref contextGUIDS,
            	(int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
            	(int)vsCommandStyle.vsCommandStylePictAndText,
            	vsCommandControlType.vsCommandControlTypeButton);

			cmdObj.AddControl(subMenuCommandBar, 1);

            // BDS 11/21/2005: Add this menu item to the web project 
            // template.
			cmdObj.AddControl(webSubMenuCommandBar, 1);

            AppLog.LogMessage("Create WSDL interface desc menu item is added");
            return contextGUIDS;
        }


        public object[] GenerateMenuItemForEditWsdl(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS)
        {
            Command cmdObj = commands.AddNamedCommand2(
            	addInInstance,
            	"EditWsdlContextMenu",
            	"Edit WSDL Interface Description...",
            	"Executes the command for WsContractFirstAddin ContextMenu",
            	true,
            	190,
            	ref contextGUIDS,
            	(int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
            	(int)vsCommandStyle.vsCommandStylePictAndText,
            	vsCommandControlType.vsCommandControlTypeButton);

			cmdObj.AddControl(subMenuCommandBar, 2);

            // BDS 11/21/2005: Add this menu item to the web project 
            // template.
			cmdObj.AddControl(webSubMenuCommandBar, 2);

            AppLog.LogMessage("Edit WSDL menu item is added");
            return contextGUIDS;
        }


        public object[] GenerateMenuItemForChooseWsdlToImplement(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS)
        {
        	Command cmdObj = commands.AddNamedCommand2(
            	addInInstance,
            	"WsContractFirstContextMenu2",
            	"Choose WSDL to Implement...",
            	"Executes the command for WsContractFirstAddin ContextMenu",
            	true,
            	190,
            	ref contextGUIDS,
            	(int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
            	(int)vsCommandStyle.vsCommandStylePictAndText,
            	vsCommandControlType.vsCommandControlTypeButton);

			cmdObj.AddControl(projectSubMenuCommandBar, 1);

            // BDS 11/21/2005: Add this menu item to the web project 
            // template. 
			cmdObj.AddControl(webProjectSubMenuCommandBar, 1);

            AppLog.LogMessage("Choose WSDL menu item is added");
            return contextGUIDS;
        }

        public object[] GenerateMenuItemForDCCodeGen(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, object[] contextGUIDS)
        {
        	Command cmdObj = commands.AddNamedCommand2(
            	addInInstance,
            	"GenerateCodeMenu",
            	"Generate Data Contract Code...",
            	"Executes the command for WsContractFirstAddin ContextMenu",
            	true,
            	190,
            	ref contextGUIDS,
            	(int)vsCommandStatus.vsCommandStatusUnsupported + (int)vsCommandStatus.vsCommandStatusEnabled,
            	(int)vsCommandStyle.vsCommandStylePictAndText,
            	vsCommandControlType.vsCommandControlTypeButton);

			cmdObj.AddControl(subMenuCommandBar, 2);

            // BDS 11/21/2005: Add this menu item to the web project 
            // template. 
			cmdObj.AddControl(webSubMenuCommandBar, 2);

            AppLog.LogMessage("Generate code menu item is added");
        	return contextGUIDS;
        }

		public void GenerateMenuItemForPasteXmlAsSchema(AddIn addInInstance, DTE2 applicationObject, Commands2 commands, CommandBars commandBars, object[] contextGUIDS)
		{
			Command cmdObj = commands.AddNamedCommand2(
				addInInstance,
				"PasteSchemaMenu",
				"Paste XML as Schema",
				"Pastes the XML on the clipboard as XSD schema.",
				true,
				239,
				ref contextGUIDS,
				(int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
				(int)vsCommandStyle.vsCommandStylePictAndText,
				vsCommandControlType.vsCommandControlTypeButton);

			CommandBar menuBarCommandBar = commandBars["MenuBar"];

			CommandBarControl editControl = menuBarCommandBar.Controls["Edit"];
			CommandBarPopup editPopup = (CommandBarPopup)editControl;
			CommandBarControl pasteControl = editPopup.CommandBar.Controls["Paste"];

			cmdObj.AddControl(editPopup.CommandBar, pasteControl != null ? pasteControl.Index + 1 : 1);

			AppLog.LogMessage("Paste Schema code menu item is added");
		}

		public void CleanUp()
		{
			if (selectionEvents != null)
			{
				selectionEvents.OnChange -= OnSelectionChanged;
			}

			RemoveCommandBarPopup(subMenuPopup);
			RemoveCommandBarPopup(webSubMenuPopup);
			RemoveCommandBarPopup(projectSubMenuPopup);
			RemoveCommandBarPopup(webProjectSubMenuPopup);
			RemoveCommandBarPopup(toolsSubMenuPopup);

			RemoveCommand(MenuWscf);
			RemoveCommand(MenuWscfContextMenu);
			RemoveCommand(MenuEditWsdl);
			RemoveCommand(MenuCreateWsdl);
			RemoveCommand(MenuWscfContextMenu2);
			RemoveCommand(MenuGenerateDataContracts);
			RemoveCommand(MenuPasteXmlAsSchema);
		}

		private void RemoveCommand(string commandName)
		{
			Command command = null;
			try
			{
				command = visualStudio.ApplicationObject.Commands.Item(commandName, -1);
			}
			catch
			{
			}

			if (command != null)
			{
				command.Delete();
			}
		}

		private void RemoveCommandBarPopup(CommandBarPopup commandBarPopup)
		{
			if (commandBarPopup != null)
			{
				commandBarPopup.Delete(true);
			}
		}

    	private void OnSelectionChanged()
		{
			subMenuPopup.Visible = false;
			webSubMenuPopup.Visible = false;

			if (visualStudio.ApplicationObject.Version == "10.0")
			{
				subMenuPopup.CommandBar.Visible = false;
				webSubMenuPopup.CommandBar.Visible = false;
			}

			foreach (VisualStudioSelectedItem selectedItem in visualStudio.SelectedItems)
			{
				if (selectedItem.FileName == null) continue;

				if (selectedItem.FileName.EndsWith(".wsdl", StringComparison.OrdinalIgnoreCase)
					|| selectedItem.FileName.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase))
				{
					subMenuPopup.Visible = true;
					webSubMenuPopup.Visible = true;

					if (visualStudio.ApplicationObject.Version == "10.0")
					{
						subMenuPopup.CommandBar.Visible = true;
						webSubMenuPopup.CommandBar.Visible = true;
					}

					break;
				}
			}
		}

    	#region "Menu status handlers"


        #endregion

    }
}
