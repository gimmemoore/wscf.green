using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;

using Thinktecture.Tools.Web.Services.Wscf.Environment;
using Thinktecture.Tools.Web.Services.CodeGeneration;

namespace Thinktecture.Tools.Wscf.UI.Dialogs
{
	/// <summary>
	/// Summary description for WebServiceCodeGenOptions.
	/// </summary>
	public partial class WebServiceCodeGenDialogNew : Form
	{
		private Button btnGenerate;
		private Button btnCancel;
		private GroupBox groupBox2;
		private Label label1;
		private TextBox tbDestinationFilename;
        private Label label2;
        private TextBox tbDestinationNamespace;
		private ToolTip cfTooltip;
		private Panel panel1;
		private GroupBox groupBox5;
		private Label label4;
		private Button bnBrowse;
		private OpenFileDialog openFileDialogWSDL;
		private IContainer components;		
		private CheckBox cbSettings;
		private CheckBox cbSeperateFiles;
		private PictureBox pbWizard;
		private ComboBox cbWsdlLocation;
		private System.Windows.Forms.CheckBox cbOverwrite;
		private System.Windows.Forms.PictureBox pbWscf;
		private System.Windows.Forms.CheckBox cbAdjustCasing;
        private System.Windows.Forms.CheckBox cbMultipleFiles;
		private System.Windows.Forms.CheckBox cbCollections;
        private System.Windows.Forms.CheckBox cbFormatSoapActions;
		private System.Windows.Forms.CheckBox cbProperties;
		private System.Windows.Forms.RadioButton rbServer;
		private System.Windows.Forms.RadioButton rbClient;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox4;
        private CheckBox cbOrderIds;
        private CheckBox cbAsync;
        private CheckBox cbDataBinding;
        
        private CheckBox cbGenericList;

        private CheckBox cbEnableWsdlEndpoint;
		private GroupBox gbServiceBehavior;
		private CheckBox cbUseSynchronizationContext;
		private Label label3;
		private Label label5;
		private ComboBox cbConcurrencyMode;
		private ComboBox cbInstanceContextMode;
		private CheckBox cbGenerateSvcFile;
        private bool isLoading = true;
        private GroupBox gbServiceMethodImplementation;
        private RadioButton rbAbstractMethods;
        private RadioButton rbPartialClassMethodCalls;
        private RadioButton rbNotImplementedException;
	    private CodeGenerationOptions options;

		public WebServiceCodeGenDialogNew()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			// Initialize the .wsdl file cache.
			wsdlFileCache = new ArrayList();
            options= new CodeGenerationOptions();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebServiceCodeGenDialogNew));
            this.cbSeperateFiles = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDestinationNamespace = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDestinationFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cfTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.cbSettings = new System.Windows.Forms.CheckBox();
            this.pbWscf = new System.Windows.Forms.PictureBox();
            this.cbAdjustCasing = new System.Windows.Forms.CheckBox();
            this.cbMultipleFiles = new System.Windows.Forms.CheckBox();
            this.cbCollections = new System.Windows.Forms.CheckBox();
            this.cbFormatSoapActions = new System.Windows.Forms.CheckBox();
            this.cbProperties = new System.Windows.Forms.CheckBox();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.rbClient = new System.Windows.Forms.RadioButton();
            this.cbOverwrite = new System.Windows.Forms.CheckBox();
            this.cbAsync = new System.Windows.Forms.CheckBox();
            this.cbDataBinding = new System.Windows.Forms.CheckBox();
            this.cbOrderIds = new System.Windows.Forms.CheckBox();
            this.cbGenericList = new System.Windows.Forms.CheckBox();
            this.cbEnableWsdlEndpoint = new System.Windows.Forms.CheckBox();
            this.cbUseSynchronizationContext = new System.Windows.Forms.CheckBox();
            this.cbConcurrencyMode = new System.Windows.Forms.ComboBox();
            this.cbInstanceContextMode = new System.Windows.Forms.ComboBox();
            this.cbGenerateSvcFile = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbWizard = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbWsdlLocation = new System.Windows.Forms.ComboBox();
            this.bnBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialogWSDL = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbServiceBehavior = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gbServiceMethodImplementation = new System.Windows.Forms.GroupBox();
            this.rbAbstractMethods = new System.Windows.Forms.RadioButton();
            this.rbPartialClassMethodCalls = new System.Windows.Forms.RadioButton();
            this.rbNotImplementedException = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWscf)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWizard)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbServiceBehavior.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gbServiceMethodImplementation.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbSeperateFiles
            // 
            this.cbSeperateFiles.Location = new System.Drawing.Point(0, 0);
            this.cbSeperateFiles.Name = "cbSeperateFiles";
            this.cbSeperateFiles.Size = new System.Drawing.Size(104, 24);
            this.cbSeperateFiles.TabIndex = 0;
            this.cfTooltip.SetToolTip(this.cbSeperateFiles, "Generates collection-based members instead of arrays.");
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(401, 628);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(77, 28);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(484, 628);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            // 
            // tbDestinationNamespace
            // 
            this.tbDestinationNamespace.Location = new System.Drawing.Point(152, 49);
            this.tbDestinationNamespace.Name = "tbDestinationNamespace";
            this.tbDestinationNamespace.Size = new System.Drawing.Size(392, 20);
            this.tbDestinationNamespace.TabIndex = 3;
            this.cfTooltip.SetToolTip(this.tbDestinationNamespace, "Please enter the name of .NET namespace for the client proxy.");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbDestinationNamespace);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbDestinationFilename);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 493);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(553, 80);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Files and namespaces  ";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Destination file name";
            // 
            // tbDestinationFilename
            // 
            this.tbDestinationFilename.Location = new System.Drawing.Point(152, 26);
            this.tbDestinationFilename.Name = "tbDestinationFilename";
            this.tbDestinationFilename.Size = new System.Drawing.Size(392, 20);
            this.tbDestinationFilename.TabIndex = 1;
            this.cfTooltip.SetToolTip(this.tbDestinationFilename, "Please enter the name of .NET proxy file that gets generated.");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Destination namespace";
            // 
            // cbSettings
            // 
            this.cbSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbSettings.Location = new System.Drawing.Point(10, 632);
            this.cbSettings.Name = "cbSettings";
            this.cbSettings.Size = new System.Drawing.Size(128, 23);
            this.cbSettings.TabIndex = 3;
            this.cbSettings.Text = "Remember settings";
            this.cfTooltip.SetToolTip(this.cbSettings, "Save dialog settings for future use.");
            this.cbSettings.CheckedChanged += new System.EventHandler(this.cbSettings_CheckedChanged);
            // 
            // pbWscf
            // 
            this.pbWscf.Image = ((System.Drawing.Image)(resources.GetObject("pbWscf.Image")));
            this.pbWscf.Location = new System.Drawing.Point(462, 4);
            this.pbWscf.Name = "pbWscf";
            this.pbWscf.Size = new System.Drawing.Size(104, 32);
            this.pbWscf.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbWscf.TabIndex = 11;
            this.pbWscf.TabStop = false;
            this.cfTooltip.SetToolTip(this.pbWscf, "http://www.thinktecture.com/");
            this.pbWscf.Click += new System.EventHandler(this.pbWscf_Click);
            // 
            // cbAdjustCasing
            // 
            this.cbAdjustCasing.Location = new System.Drawing.Point(16, 76);
            this.cbAdjustCasing.Name = "cbAdjustCasing";
            this.cbAdjustCasing.Size = new System.Drawing.Size(102, 24);
            this.cbAdjustCasing.TabIndex = 2;
            this.cbAdjustCasing.Text = "Adjust casing";
            this.cfTooltip.SetToolTip(this.cbAdjustCasing, "Ensures that generated .NET types follow the .NET guidelines for casing.");
            // 
            // cbMultipleFiles
            // 
            this.cbMultipleFiles.Location = new System.Drawing.Point(425, 49);
            this.cbMultipleFiles.Name = "cbMultipleFiles";
            this.cbMultipleFiles.Size = new System.Drawing.Size(96, 24);
            this.cbMultipleFiles.TabIndex = 8;
            this.cbMultipleFiles.Text = "Separate files";
            this.cfTooltip.SetToolTip(this.cbMultipleFiles, "Generates each data type into its own seperate source file.");
            // 
            // cbCollections
            // 
            this.cbCollections.Location = new System.Drawing.Point(299, 23);
            this.cbCollections.Name = "cbCollections";
            this.cbCollections.Size = new System.Drawing.Size(80, 24);
            this.cbCollections.TabIndex = 5;
            this.cbCollections.Text = "Collections";
            this.cfTooltip.SetToolTip(this.cbCollections, "Generates collection-based members instead of arrays.");
            this.cbCollections.CheckedChanged += new System.EventHandler(this.cbCollections_CheckedChanged);
            // 
            // cbFormatSoapActions
            // 
            this.cbFormatSoapActions.Location = new System.Drawing.Point(144, 25);
            this.cbFormatSoapActions.Name = "cbFormatSoapActions";
            this.cbFormatSoapActions.Size = new System.Drawing.Size(128, 20);
            this.cbFormatSoapActions.TabIndex = 3;
            this.cbFormatSoapActions.Text = "FormatSoapActions";
            this.cfTooltip.SetToolTip(this.cbFormatSoapActions, "Set the Action and ReplyActions according to standards");
            // 
            // cbProperties
            // 
            this.cbProperties.Location = new System.Drawing.Point(16, 24);
            this.cbProperties.Name = "cbProperties";
            this.cbProperties.Size = new System.Drawing.Size(115, 23);
            this.cbProperties.TabIndex = 0;
            this.cbProperties.Text = "Public properties";
            this.cfTooltip.SetToolTip(this.cbProperties, "Generate public properties in your data classes instead of public fields.");
            // 
            // rbServer
            // 
            this.rbServer.Location = new System.Drawing.Point(148, 19);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(112, 24);
            this.rbServer.TabIndex = 1;
            this.rbServer.Text = "Service-side stub";
            this.cfTooltip.SetToolTip(this.rbServer, "Select this to generate the service-side stub");
            this.rbServer.CheckedChanged += new System.EventHandler(this.rbServer_CheckedChanged);
            // 
            // rbClient
            // 
            this.rbClient.Location = new System.Drawing.Point(17, 19);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(111, 24);
            this.rbClient.TabIndex = 0;
            this.rbClient.Text = "Client-side proxy";
            this.cfTooltip.SetToolTip(this.rbClient, "Select this to generate the client-side proxy");
            this.rbClient.CheckedChanged += new System.EventHandler(this.rbClient_CheckedChanged);
            // 
            // cbOverwrite
            // 
            this.cbOverwrite.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbOverwrite.Location = new System.Drawing.Point(136, 631);
            this.cbOverwrite.Name = "cbOverwrite";
            this.cbOverwrite.Size = new System.Drawing.Size(144, 24);
            this.cbOverwrite.TabIndex = 4;
            this.cbOverwrite.Text = "Overwrite existing files";
            this.cfTooltip.SetToolTip(this.cbOverwrite, "Overwrite all files upon code generation.");
            this.cbOverwrite.CheckedChanged += new System.EventHandler(this.cbOverwrite_CheckedChanged);
            // 
            // cbAsync
            // 
            this.cbAsync.AutoSize = true;
            this.cbAsync.Location = new System.Drawing.Point(299, 53);
            this.cbAsync.Name = "cbAsync";
            this.cbAsync.Size = new System.Drawing.Size(98, 17);
            this.cbAsync.TabIndex = 6;
            this.cbAsync.Text = "Async methods";
            this.cfTooltip.SetToolTip(this.cbAsync, "Creates Begin and End methods for the asynchronous invocation of Web Services.");
            this.cbAsync.UseVisualStyleBackColor = true;
            // 
            // cbDataBinding
            // 
            this.cbDataBinding.AutoSize = true;
            this.cbDataBinding.Location = new System.Drawing.Point(16, 53);
            this.cbDataBinding.Name = "cbDataBinding";
            this.cbDataBinding.Size = new System.Drawing.Size(86, 17);
            this.cbDataBinding.TabIndex = 1;
            this.cbDataBinding.Text = "Data binding";
            this.cfTooltip.SetToolTip(this.cbDataBinding, "Implement INotifyPropertyChanged interface on all generated types to enable data " +
                    "binding.");
            this.cbDataBinding.UseVisualStyleBackColor = true;
            this.cbDataBinding.CheckedChanged += new System.EventHandler(this.cbDataBinding_CheckedChanged);
            // 
            // cbOrderIds
            // 
            this.cbOrderIds.AutoSize = true;
            this.cbOrderIds.Location = new System.Drawing.Point(144, 53);
            this.cbOrderIds.Name = "cbOrderIds";
            this.cbOrderIds.Size = new System.Drawing.Size(99, 17);
            this.cbOrderIds.TabIndex = 4;
            this.cbOrderIds.Text = "Order identifiers";
            this.cfTooltip.SetToolTip(this.cbOrderIds, "Generate explicit order identifiers on particle members.");
            this.cbOrderIds.UseVisualStyleBackColor = true;
            // 
            // cbGenericList
            // 
            this.cbGenericList.AutoSize = true;
            this.cbGenericList.Location = new System.Drawing.Point(425, 27);
            this.cbGenericList.Name = "cbGenericList";
            this.cbGenericList.Size = new System.Drawing.Size(61, 17);
            this.cbGenericList.TabIndex = 7;
            this.cbGenericList.Text = "List<T>";
            this.cfTooltip.SetToolTip(this.cbGenericList, "Generates List<T>-based members instead of arrays.");
            this.cbGenericList.UseVisualStyleBackColor = true;
            this.cbGenericList.CheckedChanged += new System.EventHandler(this.cbGenericList_CheckedChanged);
            // 
            // cbEnableWsdlEndpoint
            // 
            this.cbEnableWsdlEndpoint.Location = new System.Drawing.Point(299, 49);
            this.cbEnableWsdlEndpoint.Name = "cbEnableWsdlEndpoint";
            this.cbEnableWsdlEndpoint.Size = new System.Drawing.Size(160, 24);
            this.cbEnableWsdlEndpoint.TabIndex = 5;
            this.cbEnableWsdlEndpoint.Text = "Enable WSDL Endpoint";
            this.cfTooltip.SetToolTip(this.cbEnableWsdlEndpoint, "Adds the configuration required to expose the WSDL file as metadata service endpo" +
                    "int.");
            // 
            // cbUseSynchronizationContext
            // 
            this.cbUseSynchronizationContext.Checked = true;
            this.cbUseSynchronizationContext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseSynchronizationContext.Location = new System.Drawing.Point(299, 22);
            this.cbUseSynchronizationContext.Name = "cbUseSynchronizationContext";
            this.cbUseSynchronizationContext.Size = new System.Drawing.Size(191, 24);
            this.cbUseSynchronizationContext.TabIndex = 4;
            this.cbUseSynchronizationContext.Text = "Use Synchronization Context ";
            this.cfTooltip.SetToolTip(this.cbUseSynchronizationContext, "Specifies whether to use the current synchronization context to choose the thread" +
                    " of execution. ");
            this.cbUseSynchronizationContext.UseVisualStyleBackColor = true;
            // 
            // cbConcurrencyMode
            // 
            this.cbConcurrencyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConcurrencyMode.FormattingEnabled = true;
            this.cbConcurrencyMode.Items.AddRange(new object[] {
            "Single",
            "Multiple",
            "Reentrant"});
            this.cbConcurrencyMode.Location = new System.Drawing.Point(139, 24);
            this.cbConcurrencyMode.Name = "cbConcurrencyMode";
            this.cbConcurrencyMode.Size = new System.Drawing.Size(121, 21);
            this.cbConcurrencyMode.TabIndex = 1;
            this.cfTooltip.SetToolTip(this.cbConcurrencyMode, "Determines whether a service supports one thread, multiple threads, or reentrant " +
                    "calls. ");
            // 
            // cbInstanceContextMode
            // 
            this.cbInstanceContextMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstanceContextMode.FormattingEnabled = true;
            this.cbInstanceContextMode.Items.AddRange(new object[] {
            "PerCall",
            "PerSession",
            "Single"});
            this.cbInstanceContextMode.Location = new System.Drawing.Point(139, 51);
            this.cbInstanceContextMode.Name = "cbInstanceContextMode";
            this.cbInstanceContextMode.Size = new System.Drawing.Size(121, 21);
            this.cbInstanceContextMode.TabIndex = 3;
            this.cfTooltip.SetToolTip(this.cbInstanceContextMode, "Specifies the number of service instances available for handling calls that are c" +
                    "ontained in incoming messages. ");
            // 
            // cbGenerateSvcFile
            // 
            this.cbGenerateSvcFile.AutoSize = true;
            this.cbGenerateSvcFile.Location = new System.Drawing.Point(299, 79);
            this.cbGenerateSvcFile.Name = "cbGenerateSvcFile";
            this.cbGenerateSvcFile.Size = new System.Drawing.Size(109, 17);
            this.cbGenerateSvcFile.TabIndex = 6;
            this.cbGenerateSvcFile.Text = "Generate .svc file";
            this.cfTooltip.SetToolTip(this.cbGenerateSvcFile, "Determines if a .svc file will be generated for hosting in IIS and WAS.");
            this.cbGenerateSvcFile.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pbWscf);
            this.panel1.Controls.Add(this.pbWizard);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 40);
            this.panel1.TabIndex = 10;
            // 
            // pbWizard
            // 
            this.pbWizard.Image = ((System.Drawing.Image)(resources.GetObject("pbWizard.Image")));
            this.pbWizard.Location = new System.Drawing.Point(10, 3);
            this.pbWizard.Name = "pbWizard";
            this.pbWizard.Size = new System.Drawing.Size(40, 32);
            this.pbWizard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbWizard.TabIndex = 10;
            this.pbWizard.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbWsdlLocation);
            this.groupBox5.Controls.Add(this.bnBrowse);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(8, 48);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(553, 49);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Contract information  ";
            // 
            // cbWsdlLocation
            // 
            this.cbWsdlLocation.Enabled = false;
            this.cbWsdlLocation.Location = new System.Drawing.Point(96, 18);
            this.cbWsdlLocation.MaxDropDownItems = 10;
            this.cbWsdlLocation.Name = "cbWsdlLocation";
            this.cbWsdlLocation.Size = new System.Drawing.Size(404, 21);
            this.cbWsdlLocation.TabIndex = 1;
            this.cbWsdlLocation.SelectedIndexChanged += new System.EventHandler(this.cbWsdlLocation_SelectedIndexChanged);
            this.cbWsdlLocation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cbWsdlLocation_MouseMove);
            this.cbWsdlLocation.TextChanged += new System.EventHandler(this.tbWSDLLocation_TextChanged);
            // 
            // bnBrowse
            // 
            this.bnBrowse.Location = new System.Drawing.Point(512, 17);
            this.bnBrowse.Name = "bnBrowse";
            this.bnBrowse.Size = new System.Drawing.Size(32, 23);
            this.bnBrowse.TabIndex = 2;
            this.bnBrowse.Text = "...";
            this.bnBrowse.Click += new System.EventHandler(this.bnBrowse_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "WSDL location:";
            // 
            // openFileDialogWSDL
            // 
            this.openFileDialogWSDL.Filter = "WSDL files|*.wsdl|All Files|*.*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbServiceMethodImplementation);
            this.groupBox1.Controls.Add(this.gbServiceBehavior);
            this.groupBox1.Controls.Add(this.rbServer);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.rbClient);
            this.groupBox1.Location = new System.Drawing.Point(8, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 279);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Code generation  ";
            // 
            // gbServiceBehavior
            // 
            this.gbServiceBehavior.Controls.Add(this.cbGenerateSvcFile);
            this.gbServiceBehavior.Controls.Add(this.cbEnableWsdlEndpoint);
            this.gbServiceBehavior.Controls.Add(this.cbUseSynchronizationContext);
            this.gbServiceBehavior.Controls.Add(this.label3);
            this.gbServiceBehavior.Controls.Add(this.label5);
            this.gbServiceBehavior.Controls.Add(this.cbConcurrencyMode);
            this.gbServiceBehavior.Controls.Add(this.cbInstanceContextMode);
            this.gbServiceBehavior.Enabled = false;
            this.gbServiceBehavior.Location = new System.Drawing.Point(8, 162);
            this.gbServiceBehavior.Name = "gbServiceBehavior";
            this.gbServiceBehavior.Size = new System.Drawing.Size(536, 107);
            this.gbServiceBehavior.TabIndex = 3;
            this.gbServiceBehavior.TabStop = false;
            this.gbServiceBehavior.Text = "Service Behavior";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Concurrency Mode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Instance Context Mode";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbGenericList);
            this.groupBox4.Controls.Add(this.cbAsync);
            this.groupBox4.Controls.Add(this.cbDataBinding);
            this.groupBox4.Controls.Add(this.cbOrderIds);
            this.groupBox4.Controls.Add(this.cbCollections);
            this.groupBox4.Controls.Add(this.cbMultipleFiles);
            this.groupBox4.Controls.Add(this.cbProperties);
            this.groupBox4.Controls.Add(this.cbAdjustCasing);
            this.groupBox4.Controls.Add(this.cbFormatSoapActions);
            this.groupBox4.Location = new System.Drawing.Point(8, 49);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(536, 107);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options  ";
            // 
            // gbServiceMethodImplementation
            // 
            this.gbServiceMethodImplementation.Controls.Add(this.rbAbstractMethods);
            this.gbServiceMethodImplementation.Controls.Add(this.rbPartialClassMethodCalls);
            this.gbServiceMethodImplementation.Controls.Add(this.rbNotImplementedException);
            this.gbServiceMethodImplementation.Location = new System.Drawing.Point(15, 385);
            this.gbServiceMethodImplementation.Name = "gbServiceMethodImplementation";
            this.gbServiceMethodImplementation.Size = new System.Drawing.Size(536, 95);
            this.gbServiceMethodImplementation.TabIndex = 11;
            this.gbServiceMethodImplementation.TabStop = false;
            this.gbServiceMethodImplementation.Text = "Service Method Implementation";
            this.cfTooltip.SetToolTip(this.gbServiceMethodImplementation, "Determines if the operation methods on the service class will throw a NotImplemen" +
                    "tedException, call an implementation method in a partial class, or will be defin" +
                    "ed as abstract methods.");
            // 
            // rbAbstractMethods
            // 
            this.rbAbstractMethods.AutoSize = true;
            this.rbAbstractMethods.Location = new System.Drawing.Point(16, 66);
            this.rbAbstractMethods.Name = "rbAbstractMethods";
            this.rbAbstractMethods.Size = new System.Drawing.Size(508, 20);
            this.rbAbstractMethods.TabIndex = 2;
            this.rbAbstractMethods.Text = "Generate an abstract service class and abstract methods that can be implemented i" +
                "n a derived class.";
            this.rbAbstractMethods.UseVisualStyleBackColor = true;
            // 
            // rbPartialClassMethodCalls
            // 
            this.rbPartialClassMethodCalls.AutoSize = true;
            this.rbPartialClassMethodCalls.Location = new System.Drawing.Point(16, 43);
            this.rbPartialClassMethodCalls.Name = "rbPartialClassMethodCalls";
            this.rbPartialClassMethodCalls.Size = new System.Drawing.Size(504, 20);
            this.rbPartialClassMethodCalls.TabIndex = 1;
            this.rbPartialClassMethodCalls.Text = "Generate a partial service class with calls to partial methods that must be imple" +
                "mented in another file.";
            this.rbPartialClassMethodCalls.UseVisualStyleBackColor = true;
            // 
            // rbNotImplementedException
            // 
            this.rbNotImplementedException.AutoSize = true;
            this.rbNotImplementedException.Checked = true;
            this.rbNotImplementedException.Location = new System.Drawing.Point(16, 20);
            this.rbNotImplementedException.Name = "rbNotImplementedException";
            this.rbNotImplementedException.Size = new System.Drawing.Size(498, 20);
            this.rbNotImplementedException.TabIndex = 0;
            this.rbNotImplementedException.TabStop = true;
            this.rbNotImplementedException.Text = "Generate a regular service class with methods that throw a NotImplementedExceptio" +
                "n in their body.";
            this.rbNotImplementedException.UseVisualStyleBackColor = true;
            // 
            // WebServiceCodeGenDialogNew
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(570, 664);
            this.Controls.Add(this.gbServiceMethodImplementation);
            this.Controls.Add(this.cbOverwrite);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cbSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebServiceCodeGenDialogNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WSCF.blue Code Generation 1.0 ";
            this.Load += new System.EventHandler(this.WebServiceCodeGenOptions_Load);
            this.Closed += new System.EventHandler(this.WebServiceCodeGenOptions_Closed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWscf)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbWizard)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbServiceBehavior.ResumeLayout(false);
            this.gbServiceBehavior.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gbServiceMethodImplementation.ResumeLayout(false);
            this.gbServiceMethodImplementation.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnGenerate_Click(object sender, EventArgs e)
		{
		    
			if(rbClient.Checked || rbServer.Checked)
			{
                HandleClientOrServerGeneration();
			}
			else
			{
				this.DialogResult = DialogResult.None;
				btnGenerate.DialogResult = DialogResult.None;
                DisplayMessage("Please choose code generation options");
			}
		}  
     
        // TODO: These validations should be part of the service which
        // returns an appropriate error that is then wrapped in a message box

        private bool ValidateWsdlLocation()
        {
            if (cbWsdlLocation.Text.Length == 0)
            {
                DisplayMessage("Invalid WSDL Location");
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateDestinationNamespace()
        {
            if(tbDestinationNamespace.Text.Length == 0  ||	
                !ValidationHelper.IsDotNetNamespace(tbDestinationNamespace.Text))
            {
                DisplayMessage("Error in destination namespace. Please enter valid values");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateDestinationFileName()
        {
            if(tbDestinationFilename.Text.Length == 0 ||
                !ValidationHelper.IsWindowsFileName(tbDestinationFilename.Text))
            {
                DisplayMessage("Error in destination file name. Please enter valid file name.");
                return false;
            }
            else
            {
                return true;
            }

        }

        private void DisplayMessage(string message)
        {
            MessageBox.Show(message , "Web Services Contract-First code generation", 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

		private void WebServiceCodeGenOptions_Load(object sender, EventArgs e)
		{
			if(wsdlLocation.Length == 0)
			{
				cbWsdlLocation.Enabled = true;
				cbWsdlLocation.Focus();
			}
		
			if(!cbWsdlLocation.Enabled) bnBrowse.Enabled = false;

			cbConcurrencyMode.SelectedIndex = 0;
			cbInstanceContextMode.SelectedIndex = 0;

			LoadFormValues();

			if(rbClient.Checked || rbServer.Checked) btnGenerate.Enabled = true;
            isLoading = false;

			gbServiceBehavior.DataBindings.Add("Enabled", rbServer, "Checked");
            gbServiceMethodImplementation.DataBindings.Add("Enabled", rbServer, "Checked");

		}

		private void ttPicBox_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.thinktecture.com/");
		}

		private void bnBrowse_Click(object sender, EventArgs e)
		{
			if(openFileDialogWSDL.ShowDialog() == DialogResult.OK)
			{				
				AddWsdlFileToCache(openFileDialogWSDL.FileName);
			}
		}
		


		private void ttPicBox_MouseEnter(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.Hand;
		}

		private void ttPicBox_MouseLeave(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.Default;
		}

		private void ttPicBox_MouseMove(object sender, MouseEventArgs e)
		{			
		}

		private void tbWSDLLocation_TextChanged(object sender, EventArgs e)
		{			
		}

		private void WebServiceCodeGenOptions_Closed(object sender, EventArgs e)
		{
			// BDS: Save the values only if the OK button is clicked.
			if(this.DialogResult == DialogResult.OK)
			{
				SaveFormValues();		
			}
		}

		private void cbWsdlLocation_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
		
		/// <summary>
		/// Disply the location of the WSDL when moving the mouse over the combo box.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void cbWsdlLocation_MouseMove(object sender, MouseEventArgs e)
		{
			if(cbWsdlLocation.SelectedIndex >= 0)
			{
				cfTooltip.SetToolTip(cbWsdlLocation, 
					wsdlFileCache[cbWsdlLocation.SelectedIndex].ToString());		
			}
		}


		private void cbOverwrite_CheckedChanged(object sender, System.EventArgs e)
		{
            if (!isLoading)
            {
                if (cbOverwrite.Checked)
                {
                    if (MessageBox.Show(this,
                        "This will overwrite the existing files in the project. Are you sure you want to enable this option anyway?",
                        "Web Services Contract-First code generation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                    {
                        cbOverwrite.Checked = false;
                    }
                }
            }
		}

		private void pbWscf_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://www.thinktecture.com/");
		}        


        private void cbDataBinding_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbProperties.Checked)
            {
                cbProperties.Checked = true;
                cbProperties.CheckState = CheckState.Indeterminate;
                cbProperties.Enabled = false;
            }
            else
            {
                if (cbProperties.Checked && 
                    cbProperties.CheckState == CheckState.Indeterminate)
                {
                    cbProperties.Checked = false;
                }
                cbProperties.Enabled = true;
            }
        }

        private void cbSettings_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cbCollections_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGenericList.Checked)
            {
                cbGenericList.Checked = !cbCollections.Checked;
            }
        }

        private void cbGenericList_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCollections.Checked)
            {
                cbCollections.Checked = !cbGenericList.Checked;
            }
        }

        private void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            btnGenerate.Enabled = true;
        }

        private void rbServer_CheckedChanged(object sender, EventArgs e)
        {
            btnGenerate.Enabled = true;
        }
	}
}
