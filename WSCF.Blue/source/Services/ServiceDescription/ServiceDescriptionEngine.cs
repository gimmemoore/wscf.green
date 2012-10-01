using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;
using Thinktecture.Tools.Wscf.Services.ServiceDescription.Exceptions;
using FxMessage = System.Web.Services.Description.Message;
using FxOperation = System.Web.Services.Description.Operation;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Specialized;

using Thinktecture.Tools.Web.Services.Wscf.Environment;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    
    #region ServiceDescriptionEngine class

    /// <summary>
    /// Provides static methods for WSDL generation. 
    /// </summary>
    /// <remarks>This class could not be inherited.</remarks>
    public sealed class ServiceDescriptionEngine
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of ServiceDescriptionEngine class.
        /// </summary>
        private ServiceDescriptionEngine()
        { }

        #endregion

        #region Public static methods for WSDL Construction.

        /// <summary>
        /// Generates the WSDL file for specified <see cref="InterfaceContract"/>.
        /// </summary>
        /// <param name="serviceInterfaceContract">
        /// <see cref="InterfaceContract"/> to use for the WSDL generation.
        /// </param>
        /// <param name="wsdlSaveLocation">Location to save the generated WSDL file.</param>
        /// <param name="xmlComment">XML comment to add to the top of the WSDL file.</param>
        /// <returns>The path of the WSDL file generated.</returns>
        public static string GenerateWsdl(InterfaceContract serviceInterfaceContract,
            string wsdlSaveLocation, string xmlComment)
        {
            return GenerateWsdl(serviceInterfaceContract, wsdlSaveLocation, xmlComment, null);
        }

        /// <summary>
        /// Generates the WSDL file for a specified <see cref="InterfaceContract"/>.
        /// </summary>
        /// <param name="serviceInterfaceContract">
        /// <see cref="InterfaceContract"/> to use for the WSDL generation.
        /// </param>
        /// <param name="wsdlSaveLocation">Location to save the generated WSDL file.</param>
        /// <param name="xmlComment">XML comment to add to the top of the WSDL file.</param>
        /// <param name="currWsdlLocation">Path of an existing WSDL file to overwrite with the generated 
        /// WSDL file.</param>
        /// <returns>The path of the WSDL file generated.</returns>
        /// <remarks>
        /// This methods loads the information, it receive in a <see cref="InterfaceContract"/> to
        /// a <see cref="System.Web.Services.Description.ServiceDescription"/> class, which is later 
        /// used to generate the WSDL file. The loading process takes place in several steps. <br></br>
        /// 1. Load the basic meta data from <see cref="InterfaceContract"/>.<br></br>
        /// 2. Load the schema imports in the <see cref="SchemaImports"/> collection.<br></br>
        /// 3. Load the messages in <see cref="OperationsCollection"/>.<br></br>
        /// 4. Create the WSDL Port Type.<br></br>
        /// 5. Add each operation and it's corresponding in/out messages to the Port Type.<br></br>
        /// 6. Create a WSDL Binding section and add OperationBinding for each operation.<br></br>
        /// 7. Generate the WSDL 'service' tags if required.<br></br>
        /// 8. Finally write the file to the output stream.<br></br>
        /// 
        /// This method generates <see cref="WsdlGenerationException"/> exception, if it fails to create the WSDL file.
        /// If a file is specified to overwrite with the new file, the original file is restored in case of
        /// a failure.
        /// </remarks>
        public static string GenerateWsdl(InterfaceContract serviceInterfaceContract,
            string wsdlSaveLocation, string xmlComment, string currWsdlLocation)
        {
            // delegate the wsdl data construction to another method
            string wsdlData = GenerateWsdlData(serviceInterfaceContract, xmlComment, currWsdlLocation);

            // return the file name into which the wsdl data is persisted
            return PersistWsdl(serviceInterfaceContract.ServiceName, wsdlSaveLocation, xmlComment, currWsdlLocation, wsdlData);
        }

        /// <summary>
        /// Generates the WSDL file for a specified <see cref="InterfaceContract"></see>.
        /// </summary>
        /// <param name="serviceInterfaceContract">
        /// <see cref="InterfaceContract"></see> to use for the WSDL generation.
        /// </param>
        /// <param name="xmlComment">XML comment to add to the top of the WSDL file.</param>
        /// <param name="currWsdlLocation">Path of an existing WSDL file to overwrite with the generated 
        /// WSDL file.</param>
        /// <returns>The path of the WSDL file generated.</returns>
        /// <remarks>
        /// This methods loads the information, it receive in a <see cref="InterfaceContract"></see> to
        /// a <see cref="System.Web.Services.Description.ServiceDescription"></see> class, which is later 
        /// used to generate the WSDL file. The loading process takes place in several steps. <br></br>
        /// 1. Load the basic meta data from <see cref="InterfaceContract"></see>.<br></br>
        /// 2. Load the schema imports in the <see cref="SchemaImports"></see> collection.<br></br>
        /// 3. Load the messages in <see cref="OperationsCollection"></see>.<br></br>
        /// 4. Create the WSDL Port Type.<br></br>
        /// 5. Add each operation and it's corresponding in/out messages to the Port Type.<br></br>
        /// 6. Create a WSDL Binding section and add OperationBinding for each operation.<br></br>
        /// 7. Generate the WSDL 'service' tags if required.<br></br>
        /// 8. Finally write the WSDL to the output string.<br></br>
        /// 
        /// This method generates <see cref="WsdlGenerationException"></see> exception, if it fails to create the WSDL file.
        /// If a file is specified to overwrite with the new file, the original file is restored in case of
        /// a failure.
        /// </remarks>
        public static string GenerateWsdlData(InterfaceContract serviceInterfaceContract, string xmlComment, string currWsdlLocation)
        {

            System.Web.Services.Description.ServiceDescription desc = null;

            string serviceAttributeName = "";
            string bindingName = "";
            string serviceName = "";
            string portTypeName = "";
            bool isRoundtrip = false;

            if (currWsdlLocation != null)
            {
                isRoundtrip = true;
            }

            // Load the existing WSDL if one specified.
            // TODO:(SB): Shouldn't we be using the existing WsdlLoader class here?
            if (isRoundtrip)
            {
                ResetDescription(serviceInterfaceContract, currWsdlLocation, ref desc, ref serviceAttributeName, ref bindingName, ref serviceName, ref portTypeName);
            }
            else
            {
                InitializeDescription(serviceInterfaceContract, ref desc, ref serviceAttributeName, ref bindingName, ref serviceName, ref portTypeName);
            }

            ConstructMetadata(serviceInterfaceContract, desc, serviceAttributeName);
            ConstructSchemaImports(serviceInterfaceContract, isRoundtrip, desc);
            ConstructMessages(serviceInterfaceContract, desc);
            PortType portType = ConstructPortTypes(serviceInterfaceContract, desc, portTypeName);
            List<ServiceEndpoint> endpoints = ConstructBindings(serviceInterfaceContract, desc, portTypeName, portType);
            ConstructServiceElement(serviceInterfaceContract, isRoundtrip, desc, serviceName, portTypeName, endpoints);

            //Now write the initial wsdl
            string wsdlData = WriteInitialWsdl(serviceInterfaceContract, xmlComment, desc);
            
            //Update the Wsdl with Policy Information
            // TODO: How do we know that policy information is required? Should it be an option?
            wsdlData = PolicyWriter.UpdateWsdlWithPolicyInfo(wsdlData, endpoints, portTypeName);

            // Now return the fully written Wsdl
            return wsdlData;
        
        
        }
        #endregion 


        #region "WSDL Construction - Private methods"
        
        private static void ResetDescription(InterfaceContract serviceInterfaceContract, string currWsdlLocation, ref System.Web.Services.Description.ServiceDescription desc, ref string serviceAttributeName, ref string bindingName, ref string serviceName, ref string portTypeName)
        {
            desc = System.Web.Services.Description.ServiceDescription.Read(currWsdlLocation);

            // Read the existing name values.
            serviceAttributeName = desc.Name;
            bindingName = desc.Bindings[0].Name;
            portTypeName = desc.PortTypes[0].Name;

            // Check whether we have a service element and save it's name for the 
            // future use.
            if (desc.Services.Count > 0)
            {
                serviceName = desc.Services[0].Name;
            }
            else
            {
                serviceName = serviceInterfaceContract.ServiceName + "Port"; ;
            }

            // Check for the place which has the Service name and assign the new value 
            // appropriatly.			
            if (serviceAttributeName != null && serviceAttributeName != "")
            {
                serviceAttributeName = serviceInterfaceContract.ServiceName;
            }
            else if (serviceName != null && serviceName != "")
            {
                // If the user has selected to remove the service element, 
                // use the service name in the attribute by default.
                if (serviceInterfaceContract.NeedsServiceElement)
                {
                    serviceName = serviceInterfaceContract.ServiceName;
                }
                else
                {
                    serviceAttributeName = serviceInterfaceContract.ServiceName;
                }
            }
            else if (bindingName != null && bindingName != "")
            {
                bindingName = serviceInterfaceContract.ServiceName;
            }

            // Clear the service description. But do not clear the types definitions.
            desc.Extensions.Clear();
            desc.Bindings.Clear();
            desc.Documentation = "";
            desc.Imports.Clear();
            desc.Messages.Clear();
            desc.PortTypes.Clear();
            desc.RetrievalUrl = "";

            if (desc.ServiceDescriptions != null)
            {
                desc.ServiceDescriptions.Clear();
            }

            if (!serviceInterfaceContract.NeedsServiceElement)
            {
                desc.Services.Clear();
            }
        }
        
        private static void InitializeDescription(InterfaceContract serviceInterfaceContract, ref System.Web.Services.Description.ServiceDescription desc, ref string serviceAttributeName, ref string bindingName, ref string serviceName, ref string portTypeName)
        {
            desc = new System.Web.Services.Description.ServiceDescription();

            // Create the default names.
            serviceAttributeName = serviceInterfaceContract.ServiceName;
            bindingName = serviceInterfaceContract.ServiceName;
            portTypeName = serviceInterfaceContract.ServiceName + "Interface";
            serviceName = serviceInterfaceContract.ServiceName + "Port";
        }
        
        private static void ConstructMetadata(InterfaceContract serviceInterfaceContract, System.Web.Services.Description.ServiceDescription desc, string serviceAttributeName)
        {
            if (serviceAttributeName != null && serviceAttributeName != "")
            {
                desc.Name = serviceAttributeName;
            }
            desc.TargetNamespace = serviceInterfaceContract.ServiceNamespace;
            desc.Documentation = serviceInterfaceContract.ServiceDocumentation;
        }
        
        private static void ConstructSchemaImports(InterfaceContract serviceInterfaceContract, bool isRoundTrip, System.Web.Services.Description.ServiceDescription desc)
        {
            XmlSchema typesSchema = null;

            // Are we round-tripping? Then we have to access the existing types 
            // section.
            // Otherwise we just initialize a new XmlSchema for types.
            if (isRoundTrip)
            {
                typesSchema = desc.Types.Schemas[desc.TargetNamespace];
                // if we don't have a types section belonging to the same namespace as service description
                // we take the first types section available.                
                if (typesSchema == null)
                {
                    typesSchema = desc.Types.Schemas[0];
                }
                // Remove the includes. We gonna re-add them later in this operation.
                typesSchema.Includes.Clear();
            }
            else
            {
                typesSchema = new XmlSchema();
            }

            // Add imports to the types section resolved above.
            foreach (SchemaImport import in serviceInterfaceContract.Imports)
            {
                XmlSchemaExternal importedSchema = null;
                if (import.SchemaNamespace == null || import.SchemaNamespace == "")
                {
                    importedSchema = new XmlSchemaInclude();
                }
                else
                {
                    importedSchema = new XmlSchemaImport();
                    ((XmlSchemaImport)importedSchema).Namespace = import.SchemaNamespace;
                }
                if (serviceInterfaceContract.UseAlternateLocationForImports)
                {
                    importedSchema.SchemaLocation = import.AlternateLocation;
                }
                else
                {
                    importedSchema.SchemaLocation = import.SchemaLocation;
                }
                typesSchema.Includes.Add(importedSchema);
            }

            // If we are not round-tripping we have to link the types schema we just created to 
            // the service description.
            if (!isRoundTrip)
            {
                // Finally add the type schema to the ServiceDescription.Types.Schemas collection.
                desc.Types.Schemas.Add(typesSchema);
            }
        }
        
        private static void ConstructMessages(InterfaceContract serviceInterfaceContract, System.Web.Services.Description.ServiceDescription desc)
        {
            MessageCollection msgs = desc.Messages;

            foreach (Operation op in serviceInterfaceContract.Operations)
            {
                foreach (Message msg in op.MessagesCollection)
                {
                    FxMessage tempMsg = new FxMessage();
                    tempMsg.Name = msg.Name;
                    tempMsg.Documentation = msg.Documentation;

                    MessagePart msgPart = new MessagePart();
                    msgPart.Name = Constants.DefaultMessagePartName;
                    msgPart.Element = new XmlQualifiedName(msg.Element.ElementName,
                            msg.Element.ElementNamespace);
                    tempMsg.Parts.Add(msgPart);

                    msgs.Add(tempMsg);
                }
            }
        }
 
        private static PortType ConstructPortTypes(InterfaceContract serviceInterfaceContract, System.Web.Services.Description.ServiceDescription desc, string portTypeName)
        {
            PortTypeCollection portTypes = desc.PortTypes;
            PortType portType = new PortType();
            portType.Name = portTypeName;
            portType.Documentation = serviceInterfaceContract.ServiceDocumentation;

            // Add each operation and it's corresponding in/out messages to the WSDL Port Type.
            foreach (Operation op in serviceInterfaceContract.Operations)
            {
                FxOperation tempOperation = new FxOperation();
                tempOperation.Name = op.Name;
                tempOperation.Documentation = op.Documentation;
                int i = 0;

                OperationInput operationInput = new OperationInput();
                operationInput.Message = new XmlQualifiedName(op.MessagesCollection[i].Name, desc.TargetNamespace);

                tempOperation.Messages.Add(operationInput);

                if (op.Mep == Mep.RequestResponse)
                {
                    OperationOutput operationOutput = new OperationOutput();
                    operationOutput.Message = new XmlQualifiedName(op.MessagesCollection[i + 1].Name, desc.TargetNamespace);

                    tempOperation.Messages.Add(operationOutput);
                }

                portType.Operations.Add(tempOperation);
                i++;
            }

            portTypes.Add(portType);
            return portType;
        }
        
        private static List<ServiceEndpoint> ConstructBindings(InterfaceContract serviceInterfaceContract, System.Web.Services.Description.ServiceDescription desc, string portTypeName, PortType portType)
        {
            // Here we have a list of WCF endpoints.
            // Currently we populate this list with only two endpoints that has default
            // BasicHttpBinding and default NetTcpBinding.
            List<ServiceEndpoint> endpoints = new List<ServiceEndpoint>();

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            endpoints.Add(ServiceEndpointFactory<IDummyContract>.CreateServiceEndpoint(basicHttpBinding));

            // BDS (10/22/2007): Commented out the TCP binding generation as we are not going to support this feature
            // in this version.
            //NetTcpBinding netTcpBinding = new NetTcpBinding();
            //endpoints.Add(ServiceEndpointFactory<IDummyContract>.CreateServiceEndpoint(netTcpBinding));

            // Now, for each endpoint we have to create a binding in our service description.
            foreach (ServiceEndpoint endpoint in endpoints)
            {
                // Create a WSDL BindingCollection.
                BindingCollection bindings = desc.Bindings;
                System.Web.Services.Description.Binding binding = new System.Web.Services.Description.Binding();
                binding.Name = endpoint.Name.Replace(Constants.InternalContractName, portTypeName);
                binding.Type = new XmlQualifiedName(portType.Name, desc.TargetNamespace);

                // Create Operation binding for each operation and add it the the BindingCollection.
                foreach (Operation op in serviceInterfaceContract.Operations)
                {
                    // SOAP 1.1 Operation bindings.
                    OperationBinding operationBinding1 = new OperationBinding();
                    operationBinding1.Name = op.Name;

                    InputBinding inputBinding1 = new InputBinding();
                    object bodyBindingExtension = GetSoapBodyBinding(endpoint.Binding);
                    if (bodyBindingExtension != null)
                    {
                        inputBinding1.Extensions.Add(bodyBindingExtension);
                    }
                    operationBinding1.Input = inputBinding1;

                    // Input message.
                    // Look up the message headers for each Message and add them to the current binding.
                    foreach (MessageHeader inHeader in op.MessagesCollection[0].HeadersCollection)
                    {
                        object headerBindingExtension = GetSoapHeaderBinding(endpoint.Binding, inHeader.Message, desc.TargetNamespace);
                        if (headerBindingExtension != null)
                        {
                            inputBinding1.Extensions.Add(headerBindingExtension);
                        }
                    }

                    if (op.Mep == Mep.RequestResponse)
                    {
                        // Output message.
                        OutputBinding outputBinding1 = new OutputBinding();
                        object responseBodyBindingExtension = GetSoapBodyBinding(endpoint.Binding);
                        if (responseBodyBindingExtension != null)
                        {
                            outputBinding1.Extensions.Add(responseBodyBindingExtension);
                        }
                        operationBinding1.Output = outputBinding1;

                        // Look up the message headers for each Message and add them to the current binding. 
                        foreach (MessageHeader outHeader in op.MessagesCollection[1].HeadersCollection)
                        {
                            object headerBindingExtension = GetSoapHeaderBinding(endpoint.Binding, outHeader.Message, desc.TargetNamespace);
                            if (headerBindingExtension != null)
                            {
                                outputBinding1.Extensions.Add(headerBindingExtension);
                            }
                        }
                    }

                    string action = desc.TargetNamespace + ":" + op.Input.Name;
                    object operationBindingExtension = GetSoapOperationBinding(endpoint.Binding, action);
                    if (operationBindingExtension != null)
                    {
                        operationBinding1.Extensions.Add(operationBindingExtension);
                    }

                    binding.Operations.Add(operationBinding1);
                    // End of SOAP 1.1 operation bindings.                    
                }

                object soapBindingExtension = GetSoapBinding(endpoint.Binding);
                if (soapBindingExtension != null)
                {
                    binding.Extensions.Add(soapBindingExtension);
                }
                bindings.Add(binding);
            }
            return endpoints;
        }
        
        private static void ConstructServiceElement(InterfaceContract serviceInterfaceContract, bool isRoundTrip, System.Web.Services.Description.ServiceDescription desc, string serviceName, string portTypeName, List<ServiceEndpoint> endpoints)
        {
            // Generate <service> element optionally - sometimes necessary for interop reasons
            if (serviceInterfaceContract.NeedsServiceElement)
            {
                Service defaultService = null;
                if (isRoundTrip || desc.Services.Count == 0)
                {
                    // Create a new service element.
                    defaultService = new Service();
                    defaultService.Name = serviceName;
                    foreach (ServiceEndpoint endpoint in endpoints)
                    {
                        if (endpoint.Binding.MessageVersion.Envelope == EnvelopeVersion.Soap11)
                        {
                            Port defaultPort = new Port();
                            defaultPort.Name = serviceInterfaceContract.ServiceName + "Port";
                            defaultPort.Binding = new XmlQualifiedName(endpoint.Name.Replace(Constants.InternalContractName, portTypeName), desc.TargetNamespace);
                            SoapAddressBinding defaultSoapAddressBinding = new SoapAddressBinding();
                            defaultSoapAddressBinding.Location = GetDefaultEndpoint(endpoint.Binding, serviceInterfaceContract.ServiceName);
                            defaultPort.Extensions.Add(defaultSoapAddressBinding);
                            defaultService.Ports.Add(defaultPort);
                        }
                        else if (endpoint.Binding.MessageVersion.Envelope == EnvelopeVersion.Soap12)
                        {
                            Port soap12Port = new Port();
                            soap12Port.Name = serviceInterfaceContract.ServiceName + "SOAP12Port";
                            soap12Port.Binding = new XmlQualifiedName(endpoint.Name.Replace(Constants.InternalContractName, portTypeName), desc.TargetNamespace);
                            Soap12AddressBinding soap12AddressBinding = new Soap12AddressBinding();
                            soap12AddressBinding.Location = GetDefaultEndpoint(endpoint.Binding, serviceInterfaceContract.ServiceName);
                            soap12Port.Extensions.Add(soap12AddressBinding);
                            defaultService.Ports.Add(soap12Port);
                        }
                    }

                    desc.Services.Add(defaultService);
                }
                else
                {
                    defaultService = desc.Services[0];
                    defaultService.Name = serviceName;
                }
            }
        }

        private static string WriteInitialWsdl(InterfaceContract serviceInterfaceContract, string xmlComment,System.Web.Services.Description.ServiceDescription desc)
        {
            MemoryStream outputStream  = new MemoryStream();
            StreamWriter writer1 = new StreamWriter(outputStream);
            try
            {
                XmlTextWriter writer11 = new XmlTextWriter(writer1);

                writer11.Formatting = Formatting.Indented;
                writer11.Indentation = 2;
                writer11.WriteComment(xmlComment);
                // BDS: Added a new comment line with the date time of WSDL file.
                CultureInfo ci = new CultureInfo("en-US");
                writer11.WriteComment(DateTime.Now.ToString("dddd", ci) + ", " + DateTime.Now.ToString("dd-MM-yyyy - hh:mm tt", ci));

                XmlSerializer serializer1 = System.Web.Services.Description.ServiceDescription.Serializer;
                XmlSerializerNamespaces nsSer = new XmlSerializerNamespaces();
                nsSer.Add("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
                nsSer.Add("soap12", "http://schemas.xmlsoap.org/wsdl/soap12/");
                nsSer.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                nsSer.Add("tns", desc.TargetNamespace);

                // Add the imported namespaces to the WSDL <description> element.
                for (int importIndex = 0; importIndex < serviceInterfaceContract.Imports.Count;
                    importIndex++)
                {
                    if (serviceInterfaceContract.Imports[importIndex].SchemaNamespace != null &&
                        serviceInterfaceContract.Imports[importIndex].SchemaNamespace != "")
                    {
                        nsSer.Add("import" + importIndex.ToString(),
                                  serviceInterfaceContract.Imports[importIndex].SchemaNamespace);
                    }
                }

                // Finally write the file to the output stram.
                serializer1.Serialize(writer11, desc, nsSer);

                // Close the stream 
                //writer1.Close();

                // Now write the stream into a string
                outputStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(outputStream);
                string wsdlData = sr.ReadToEnd();
                //sr.Close();
                writer1.Close();

                
                return wsdlData;
            }
            catch (Exception ex)
            {
                writer1.Close();
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += ex.InnerException.Message;
                }
                throw new WsdlGenerationException(message, ex);
            }
            

        }

        private static string PersistWsdl(string serviceName, string wsdlSaveLocation, string xmlComment, string currWsdlLocation, string wsdlData)
        {
            // Generate the WSDL file.
            string fileName = string.Empty;
            string bkFileName = string.Empty;

            // Overwrite the existing file if one specified.
            if (currWsdlLocation == null)
            {
                fileName = wsdlSaveLocation + @"\" + serviceName + ".wsdl";
            }
            else
            {
                fileName = currWsdlLocation;
            }

            // Backup existing file before proceeding.
            if (File.Exists(fileName))
            {
                bkFileName = BackupExistingFile(fileName);
            }

            try
            {
                File.WriteAllText(fileName, wsdlData);

                if (bkFileName != string.Empty)
                {
                    File.Delete(bkFileName);
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += ex.InnerException.Message;
                }

                // Restore the original file.
                if (bkFileName != string.Empty)
                {
                    RestoreFile(fileName, bkFileName, message);
                }
                else if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                throw new WsdlGenerationException(
                    message, ex);
            }

            // return the path to the wsdl file
            return fileName;
        }

        private static void RestoreFile(string fileName, string bkFileName, string message)
        {
            try
            {
                File.Copy(bkFileName, fileName, true);
                File.Delete(bkFileName);
            }
            catch
            {
                throw new WsdlGenerationException(
                    message + "\nFailed to restore the original file.");
            }
        }

        private static string BackupExistingFile(string fileName)
        {
            string bkFileName = String.Empty;

            int index = 1;
            // Create the backup file name. 
            // See whether the generated backup file name is already taken by an existing file and 
            // generate a new file name. 
            while (File.Exists(fileName + "." + index.ToString()))
            {
                index++;
            }
            bkFileName = fileName + "." + index.ToString();

            // Backup the file.
            try
            {
                File.Copy(fileName, bkFileName);
            }
            catch (Exception ex)
            {
                throw new WsdlGenerationException("An error occured while trying to generate a WSDL. Failed to backup the existing WSDL file.", ex);
            }
            return bkFileName;
        }

        #endregion

        #region "WSDL Building Helpers"

        private static object GetSoapBinding(System.ServiceModel.Channels.Binding binding)
        {
            if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap11)
            {
                SoapBinding soapBinding = new SoapBinding();
                soapBinding.Transport = GetTransport(binding);
                soapBinding.Style = SoapBindingStyle.Document;
                return soapBinding;
            }
            else if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap12)
            {
                Soap12Binding soapBinding = new Soap12Binding();
                soapBinding.Transport = GetTransport(binding);
                soapBinding.Style = SoapBindingStyle.Document;
                return soapBinding;
            }
            return null;
        }

        private static object GetSoapOperationBinding(System.ServiceModel.Channels.Binding binding, string action)
        {
            if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap11)
            {
                SoapOperationBinding soapOperationBinding = new SoapOperationBinding();
                soapOperationBinding.SoapAction = action;
                soapOperationBinding.Style = SoapBindingStyle.Document;
                return soapOperationBinding;
            }
            else if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap12)
            {
                Soap12OperationBinding soapOperationBinding = new Soap12OperationBinding();
                soapOperationBinding.SoapAction = action;
                soapOperationBinding.Style = SoapBindingStyle.Document;
                return soapOperationBinding;
            }
            return null;
        }

        private static object GetSoapHeaderBinding(System.ServiceModel.Channels.Binding binding, string message, string ns)
        {
            if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap11)
            {
                SoapHeaderBinding headerBinding = new SoapHeaderBinding();
                headerBinding.Use = SoapBindingUse.Literal;
                headerBinding.Message = new XmlQualifiedName(message, ns);
                headerBinding.Part = Constants.DefaultMessagePartName;
                return headerBinding;
            }
            else if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap12)
            {
                Soap12HeaderBinding headerBinding = new Soap12HeaderBinding();
                headerBinding.Use = SoapBindingUse.Literal;
                headerBinding.Message = new XmlQualifiedName(message, ns);
                headerBinding.Part = Constants.DefaultMessagePartName;
                return headerBinding;
            }

            return null;
        }

        private static object GetSoapBodyBinding(System.ServiceModel.Channels.Binding binding)
        {
            if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap11)
            {
                SoapBodyBinding soapBinding = new SoapBodyBinding();
                soapBinding.Use = SoapBindingUse.Literal;
                return soapBinding;
            }
            else if (binding.MessageVersion.Envelope == EnvelopeVersion.Soap12)
            {
                Soap12BodyBinding soap12Binding = new Soap12BodyBinding();
                soap12Binding.Use = SoapBindingUse.Literal;
                return soap12Binding;
            }

            return null;
        }

        private static string GetTransport(System.ServiceModel.Channels.Binding binding)
        {
            TransportBindingElement transport = binding.CreateBindingElements().Find<TransportBindingElement>();
            if (transport != null)
            {
                if (typeof(HttpTransportBindingElement) == transport.GetType())
                {
                    return "http://schemas.xmlsoap.org/soap/http";
                }
                if (typeof(HttpsTransportBindingElement) == transport.GetType())
                {
                    return "http://schemas.xmlsoap.org/soap/https";
                }
                if (typeof(TcpTransportBindingElement) == transport.GetType())
                {
                    return "http://schemas.microsoft.com/soap/tcp";
                }
                if (typeof(NamedPipeTransportBindingElement) == transport.GetType())
                {
                    return "http://schemas.microsoft.com/soap/named-pipe";
                }
                if (typeof(MsmqTransportBindingElement) == transport.GetType())
                {
                    return "http://schemas.microsoft.com/soap/msmq";
                }
            }
            return "";
        }

        private static string GetDefaultEndpoint(System.ServiceModel.Channels.Binding binding, string serviceName)
        {
            TransportBindingElement transport = binding.CreateBindingElements().Find<TransportBindingElement>();
            if (transport != null)
            {
                if (typeof(HttpTransportBindingElement) == transport.GetType())
                {
                    UriBuilder ub = new UriBuilder(transport.Scheme, "localhost");
                    ub.Path = serviceName;
                    return ub.Uri.ToString();
                }
                if (typeof(HttpsTransportBindingElement) == transport.GetType())
                {
                    UriBuilder ub = new UriBuilder(transport.Scheme, "localhost");
                    ub.Path = serviceName;
                    return ub.Uri.ToString();
                }
                if (typeof(TcpTransportBindingElement) == transport.GetType())
                {
                    UriBuilder ub = new UriBuilder(transport.Scheme, "localhost");
                    ub.Path = serviceName;
                    return ub.Uri.ToString();
                }
                if (typeof(NamedPipeTransportBindingElement) == transport.GetType())
                {
                    return "tbd";
                }
                if (typeof(MsmqTransportBindingElement) == transport.GetType())
                {
                    return "tbd";
                }
            }
            return "";
        }
        #endregion

        #region "utility methods"

        #endregion

        #endregion

        #region "Public static method to load WSDL"

        /// <summary>
        /// Creates an <see cref="InterfaceContract"/> object by loading the contents in a specified 
        /// WSDL file.
        /// </summary>
        /// <param name="wsdlFileName">Path of the WSDL file to load the information from.</param>
        /// <returns>An instance of <see cref="InterfaceContract"/> with the information loaded from the WSDL file.</returns>
        /// <remarks>
        /// This method first loads the content of the WSDL file to an instance of 
        /// <see cref="System.Web.Services.Description.ServiceDescription"/> class. Then it creates an 
        /// instance of <see cref="InterfaceContract"/> class by loading the data from that.
        /// This method throws <see cref="WsdlLoadException"/> in case of a failure to load the WSDL file.
        /// </remarks>
        public static InterfaceContract GetInterfaceContract(string wsdlFileName)
        {
            return WsdlLoader.GetInterfaceContract(wsdlFileName);
        }

        #endregion


        #region "Public methods to handle interface contract"

        #endregion
    }

    
}