using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;
using Thinktecture.Tools.Wscf.Services.ServiceDescription.Helpers;
using Thinktecture.Tools.Web.Services.Wscf.Environment;
using FxMessage = System.Web.Services.Description.Message;
using FxOperation = System.Web.Services.Description.Operation;
using FxDescription = System.ServiceModel.Description;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Specialized;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
	#region InterfaceContract class

	/// <summary>
	/// Represents the content of service description.
	/// </summary>
	/// <remarks>
	/// This type is used to explain the content of the WSDL file. WSDL generation engine use this simple 
	/// class rather than the .Net framework's ServiceDescription class to hold only the necessary details
	/// for the WSDL generation.
	/// </remarks>
	public class InterfaceContract
	{
        [Flags]
        public enum SoapBindings
        {
            Soap11 = 1,
            Soap12 = 2,
        }

		#region Private fields

	    private List<SchemaElement> usedElements;

	    private FxDescription.ServiceDescription desc;
	    
        //TODO:(SB): Revisit this area as we need to support different MEPs
        private readonly List<Operation> twoWayOperations;
	    private readonly List<Operation> oneWayOperations;
	    private Hashtable operationNamePatterns;

	    #endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the InterfaceContract class.
		/// </summary>
		public InterfaceContract()
		{
            //initialize all private members
            desc = new FxDescription.ServiceDescription();
            twoWayOperations = new List<Operation>();
            oneWayOperations = new List<Operation>();
            operationNamePatterns = new Hashtable();
			usedElements = new List<SchemaElement>();

            //initialize all public members
            IsHttpBinding = false;
            Bindings |= SoapBindings.Soap11;
            Imports = new SchemaImports();
			Types = new List<SchemaElement>();
			HeaderSchemas = new List<SchemaElement>();
			MessageSchemas = new List<SchemaElement>();
            Operations = new List<Operation>();
			SchemaImports = new List<SchemaImport>();
		}

		
		#endregion

		#region Public properties.


	    /// <summary>
		/// Gets or sets the XML name attribute of the "descriptions" tag enclosing the Web Services 
		/// Description Language (WSDL) file.
		/// </summary>
		public string ServiceName { get; set;}

		/// <summary>
		/// Gets or sets the XML targetNamespace attribute of the "descriptions" tag enclosing the 
		/// Web Services Description Language (WSDL) file.
		/// </summary>
		public string ServiceNamespace { get; set;}

	    /// <summary>
	    /// Gets or sets the Schema namespace corresponding to the service
	    /// </summary>
	    public string SchemaNamespace { get; set; }
	    
        /// <summary>
	    /// Gets or sets the text documentation for the instance of the InterfaceContract.
	    /// </summary>
	    public string ServiceDocumentation { get; set; }


        /// <summary>
        /// Gets or sets the value indicating whether the generated WSDL requires a "service" element or not.
        /// </summary>
        public bool NeedsServiceElement { get; set; }


	    /// <summary>
	    /// Gets or sets the <see cref="SchemaImports"/> for the instance of the InterfaceContract.
	    /// </summary>
	    /// <remarks>This list is placed on the WSDL by enclosing them in the "import" tags.</remarks>
	    public SchemaImports Imports { get; set; }

		public List<SchemaImport> SchemaImports { get; set; }

		public List<SchemaElement> HeaderSchemas { get; private set; }

		public List<SchemaElement> MessageSchemas { get; private set; }

	    /// <summary>
	    /// Gets the  <see cref="Operations"/> defined by the InterfaceContract.
	    /// </summary>
	    public List<Operation> Operations { get; private set; }

       
        /// <summary>
	    /// Gets the <see cref="SchemaElements"/> defined by the InterfaceContract.
	    /// </summary>
	    /// <remarks>
	    /// This property could be used only when loading an existing WSDL using <see cref="GetInterfaceContract"/> 
	    /// method. A call to <see cref="GetInterfaceContract"/> initializes this property with type definitions
	    /// embedded to WSDL.
	    /// </remarks>
		public List<SchemaElement> Types { get; private set; }

	    /// <summary>
	    /// Gets or sets a value indicating whether to use the alternative location for the schemaLocation attribute of 
	    /// "imports" tags.
	    /// </summary>
	    public bool UseAlternateLocationForImports { get; set; }

	    /// <summary>
	    /// Gets or sets a value indicating whether the current instance of InterfaceContract
	    /// class uses 'HttpBinding'.
	    /// </summary>
	    /// <remarks>
	    /// This property is set only when the instance of InterfaceContract is loaded
	    /// from an existing WSDL file.
	    /// </remarks>
	    public bool IsHttpBinding { get; set; }

	    /// <summary>
	    /// Gets or sets the SOAP bindings to be included in the WSDL.
	    /// </summary>
	    public SoapBindings Bindings { get; set; }

	    #endregion

   


        #region Internal methods

        /// <summary>
		/// Initializes the <see cref="Type"/> property.
		/// </summary>
		/// <param name="schemaElements">
		/// An instance of <see cref="SchemaElements"/> class.
		/// </param>
		/// <remarks>This method is accessible only within the assembly.</remarks>
		internal void SetTypes(List<SchemaElement> schemaElements)
		{
			this.Types.AddRange(schemaElements);
		}
		
		#endregion

        #region "public methods"

        public static bool ValidateUri(string serviceNamespace)
	    {
	        Uri uri = null;
	        bool isValid = true;

	        try
	        {
	            if (serviceNamespace.IndexOf(":") > -1 || Path.IsPathRooted(serviceNamespace))
	            {
	                uri = new Uri(serviceNamespace);
	            }
	            else
	            {
	                uri = new Uri("anyuri:" + serviceNamespace);
	            }

	        }
	        catch
	        {
	            // dont care about specific exceptions
	            isValid = false;
	        }

	        return isValid;
	    }

        public void InitializeServiceInfo(string serviceName, string serviceNamespace, string schemaNamespace, string serviceDocumentation)
        {
            this.ServiceName = serviceName;
            this.ServiceNamespace = serviceNamespace;
            this.ServiceDocumentation = serviceDocumentation;
            this.SchemaNamespace = schemaNamespace;
        }

	    public void AddSchemasToContract(NameValueCollection schemasAndLocations, string workingFolder, string projectRootFolder)
	    {
	        //TODO:(SB): Will need to have a way to add header and message schemas separately
	        string schemaNamespace = String.Empty;
	        ArrayList importedSchemaNamespaces = new ArrayList();


	        // Add the selected schemas to the ServiceInterfaceContract.Imports collection.
	        foreach (string key in schemasAndLocations.Keys)
	        {
	            string schemaName = key;
	            string schemaLocation = schemasAndLocations[key];
	            ArrayList result;
	            // Read the content of the imported files and add them to the local arrays for UI.
	            result = SchemaUtility.GetSchemasFromXsd(schemaLocation, out schemaNamespace);

	            if (schemaNamespace != null)
	            {
	                importedSchemaNamespaces.Add(schemaNamespace.ToLower());
	            }
				MessageSchemas.AddRange((List<SchemaElement>)result[1]);
				HeaderSchemas.AddRange((List<SchemaElement>)result[1]);

                SchemaImport si = new SchemaImport(schemaLocation, schemaName, schemaNamespace, workingFolder, projectRootFolder);

	            this.Imports.Add(si);
	        }

	        // Import the embedded types.
	        //ImportEmbeddedTypes();

	        // Check for the messages count found in the imported files and alert the user if no messages 
	        // are found.
	        if (MessageSchemas.Count < 1)
	        {
	            throw new Exception("There are no elements in this XSD to use as operation messages.");
	        }

	    }

        public List<Operation> InferOperations(Hashtable namePatterns)
        {
            operationNamePatterns = namePatterns;
            InferRequestResponseOperations();
            InferOneWayOperations();

            return this.Operations;


        }

	    /// <summary>
	    /// Infers the operations according to the message name patterns in the message contracts.
	    /// </summary>
	    public List<Operation> InferOperations()
	    {
	        // Define a hash table with list of request/response patterns.
	        Hashtable patterns = new Hashtable();
            SetDefaultOperationNamePatterns(patterns);
	        return this.InferOperations(patterns);
        }

        #endregion

        #region "private methods"

        private void SetDefaultOperationNamePatterns(Hashtable patterns)
	    {
	        //TODO:(SB):This will change in a future version to allow injecting  a
	        // pattern collection

	        patterns.Add("", "Response");
            patterns.Add("Request", "Response");
            patterns.Add("RequestMessage", "ResponseMessage");
	    }

	    private void InferRequestResponseOperations()
	    {
	        // Infer two-way operations.
	        foreach (SchemaElement melement in MessageSchemas)
	        {
	            foreach (string pattern in operationNamePatterns.Keys)
	            {
	                string operationName = "";
	                if (melement.ElementName.EndsWith(pattern))
	                {
	                    string responsePattern = operationNamePatterns[pattern].ToString();

	                    // Create the response element.
	                    SchemaElement responseElement = new SchemaElement();
	                    if (pattern == "")
	                    {
	                        operationName = melement.ElementName;
	                        responseElement.ElementName = melement.ElementName + responsePattern;
	                    }
	                    else
	                    {
	                        if (melement.ElementName == pattern)
	                        {
	                            operationName = melement.ElementName;
	                            responseElement.ElementName = responsePattern;
	                        }
	                        else
	                        {
	                            operationName =
	                                melement.ElementName.Substring(0,
	                                                               melement.ElementName.LastIndexOf(pattern));
	                            responseElement.ElementName =
	                                melement.ElementName.Substring(0,
	                                                               melement.ElementName.LastIndexOf(pattern)) + responsePattern;
	                        }
	                    }
	                    responseElement.ElementNamespace = melement.ElementNamespace;

	                    if (MessageSchemas.Contains(responseElement))
	                    {
	                        // Check whether the operation exists in the imported operations list.
	                        bool exists = false;
	                        foreach (Operation impOp in this.Operations)
	                        {
	                            if (impOp.Input.Element == melement &&
	                                (impOp.Output != null && impOp.Output.Element == responseElement))
	                            {
	                                exists = true;
	                                break;
	                            }
	                        }

	                        if (exists)
	                        {
	                            break;
	                        }

	                        // Add the operation to the list.
	                        Operation op = new Operation();
	                        op.Name = operationName;
	                        op.Documentation = "";
	                        op.Mep = Mep.RequestResponse;

	                        string opName = op.Name;
	                        string opNamePrefix = opName.Substring(0, 1);
	                        opNamePrefix = opNamePrefix.ToLower(CultureInfo.CurrentCulture);
	                        opName = op.Name.Substring(1, op.Name.Length - 1);
	                        opName = opNamePrefix + opName;

	                        // Add the input message.
	                        Message input = new Message();
	                        input.Name = opName + "In";
	                        input.Element = melement;
	                        input.Documentation = "";
	                        op.MessagesCollection.Add(input);
	                        op.Input = input;

	                        // Add the output message.
	                        Message output = new Message();
	                        output.Name = opName + "Out";
	                        output.Element = responseElement;
	                        output.Documentation = "";
	                        op.MessagesCollection.Add(output);
	                        op.Output = output;

	                        // Add the operation to the inferred operations collection.
	                        this.twoWayOperations.Add(op);
                            this.Operations.Add(op);

	                        // Exit this loop.
	                        break;
	                    }
	                }
	            }
	        }
	    }

	    private void InferOneWayOperations()
	    {
	        // Infer one-way operations.
	        foreach (SchemaElement melement in MessageSchemas)
	        {
	            foreach (string requestPattern in operationNamePatterns.Keys)
	            {
	                string operationName = "";
	                if (melement.ElementName.EndsWith(requestPattern))
	                {
                        // Check whether the operation exists in the imported operations list.
                        bool exists = false;
                        foreach (Operation impOp in this.Operations)
                        {
                            if (impOp.Input.Element == melement ||
                                (impOp.Output != null && impOp.Output.Element == melement))
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (exists)
                        {
                            break;
                        }

                        // Compose the operation name.
                        if (requestPattern == "")
                        {
                            operationName = melement.ElementName;
                        }
                        else
                        {
                            if (melement.ElementName == requestPattern)
                            {
                                operationName = melement.ElementName;
                            }
                            else
                            {
                                operationName =
                                    melement.ElementName.Substring(0,
                                                                   melement.ElementName.LastIndexOf(requestPattern));
                            }
                        }

                        // Add the operation to the list.
                        Operation op = new Operation();
                        op.Name = operationName;
                        op.Documentation = "";
                        op.Mep = Mep.OneWay;

                        string opName = op.Name;
                        string opNamePrefix = opName.Substring(0, 1);
                        opNamePrefix = opNamePrefix.ToLower(CultureInfo.CurrentCulture);
                        opName = op.Name.Substring(1, op.Name.Length - 1);
                        opName = opNamePrefix + opName;

                        // Add the input message.
                        Message input = new Message();
                        input.Name = opName + "In";
                        input.Element = melement;
                        input.Documentation = "";
                        op.MessagesCollection.Add(input);
                        op.Input = input;

                        // Add the operation to the inferred operations collection.
                        this.oneWayOperations.Add(op);
                        this.Operations.Add(op);


	                    // Exit this loop.
	                    break;
	                }
	            }
	        }
        }

        #endregion
    }
	
	#endregion
	

}