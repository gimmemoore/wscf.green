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


namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    public class WsdlLoader
    {
        private static System.Web.Services.Description.ServiceDescription srvDesc = null;
        private static bool isHttpBinding = true;

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
            // Try to load the service description from the specified file.
            LoadWsdl(wsdlFileName);

            // Validate the WSDL before proceeding.
            CheckIfWsdlIsUsableForRoundtripping();

            // Start building the simplified InterfaceContract object from the 
            // .Net Fx ServiceDescription we created.
            InterfaceContract simpleContract = new InterfaceContract();

            // Initialize the basic meta data.
            simpleContract.ServiceName = srvDesc.Name;
            simpleContract.ServiceNamespace = srvDesc.TargetNamespace;
            simpleContract.ServiceDocumentation = srvDesc.Documentation;
            // Try to get the service namespace from the service description.

            // If it was not found in the service description. Then try to get it from the 
            // service. If it is not found their either, then try to get it from binding.	
            if (simpleContract.ServiceName == null || simpleContract.ServiceName == "")
            {
                if (srvDesc.Services.Count > 0 && srvDesc.Services[0].Name != null &&
                    srvDesc.Services[0].Name != "")
                {
                    simpleContract.ServiceName = srvDesc.Services[0].Name;
                }
                else
                {
                    simpleContract.ServiceName = srvDesc.Bindings[0].Name;
                }
            }

            // Set the http binding property.
            simpleContract.IsHttpBinding = isHttpBinding;

            // Initialize the imports.
            foreach (XmlSchema typeSchema in srvDesc.Types.Schemas)
            {
                foreach (XmlSchemaObject schemaObject in typeSchema.Includes)
                {
                    XmlSchemaImport import = schemaObject as XmlSchemaImport;

                    if (import != null)
                    {
                        SchemaImport simpleImport = new SchemaImport();
                        simpleImport.SchemaNamespace = import.Namespace;
                        simpleImport.SchemaLocation = import.SchemaLocation;
                        simpleContract.Imports.Add(simpleImport);
                    }
                }
            }

            // Initialize the types embedded to the WSDL.
            simpleContract.SetTypes(GetSchemaElements(srvDesc.Types.Schemas, srvDesc.TargetNamespace));

            // Initialize the operations and in/out messages.
            PortType ptype = srvDesc.PortTypes[0];
            if (ptype != null)
            {
                foreach (FxOperation op in ptype.Operations)
                {
                    // Create the Operation.
                    Operation simpleOp = new Operation();
                    simpleOp.Name = op.Name;
                    simpleOp.Documentation = op.Documentation;

                    if (op.Messages.Input != null)
                    {
                        FxMessage inMessage = srvDesc.Messages[op.Messages.Input.Message.Name];

                        if (inMessage == null)
                        {
                            // WSDL modified.
                            throw new WsdlModifiedException("Could not find the message");
                        }

                        MessagePart part = inMessage.Parts[0];
                        if (part != null)
                        {
                            // Create the input message.
                            Message simpleInMessage = new Message();
                            simpleInMessage.Name = inMessage.Name;
                            simpleInMessage.Element.ElementName = part.Element.Name;
                            simpleInMessage.Element.ElementNamespace = part.Element.Namespace;
                            simpleInMessage.Documentation = inMessage.Documentation;

                            simpleOp.MessagesCollection.Add(simpleInMessage);

                            simpleOp.Input = simpleInMessage;
                        }
                        else
                        {
                            // WSDL is modified. 
                            throw new WsdlModifiedException("Could not find the message part");
                        }
                    }

                    if (op.Messages.Output != null)
                    {
                        FxMessage outMessage = srvDesc.Messages[op.Messages.Output.Message.Name];

                        if (outMessage == null)
                        {
                            // WSDL is modified.
                            throw new WsdlModifiedException("Could not find the message");
                        }

                        MessagePart part = outMessage.Parts[0];
                        if (part != null)
                        {
                            // Create the output message.
                            Message simpleOutMessage = new Message();
                            simpleOutMessage.Name = outMessage.Name;
                            simpleOutMessage.Element.ElementName = part.Element.Name;
                            simpleOutMessage.Element.ElementNamespace = part.Element.Namespace;
                            simpleOutMessage.Documentation = outMessage.Documentation;

                            simpleOp.MessagesCollection.Add(simpleOutMessage);

                            simpleOp.Output = simpleOutMessage;
                        }
                        else
                        {
                            // WSDL is modified. 
                            throw new WsdlModifiedException("Could not find the message part");
                        }

                        // Set the message direction.
                        simpleOp.Mep = Mep.RequestResponse;
                    }
                    else
                    {
                        simpleOp.Mep = Mep.OneWay;
                    }

                    // Finally add the Operation to Operations collection.
                    simpleContract.Operations.Add(simpleOp);
                }
            }
            else
            {
                // WSDL is modified.
                throw new WsdlModifiedException("Could not find the portType");
            }

            // Initialize the message headers and header messages.
            System.Web.Services.Description.Binding binding1 = srvDesc.Bindings[0];
            if (binding1 != null)
            {
                // Find the corresponding Operation in the InterfaceContract, for each OperationBinding 
                // in the binding1.Operations collection. 
                foreach (OperationBinding opBinding in binding1.Operations)
                {
                    foreach (Operation simpleOp in simpleContract.Operations)
                    {
                        if (simpleOp.Name == opBinding.Name)
                        {
                            if (opBinding.Input != null)
                            {
                                // Enumerate the message headers for the input message.
                                foreach (ServiceDescriptionFormatExtension extension in opBinding.Input.Extensions)
                                {
                                    SoapHeaderBinding inHeader = extension as SoapHeaderBinding;
                                    if (inHeader != null)
                                    {
                                        // Create the in header and add it to the headers collection.
                                        MessageHeader simpleInHeader = new MessageHeader();
                                        FxMessage inHeaderMessage = srvDesc.Messages[inHeader.Message.Name];

                                        if (inHeaderMessage == null)
                                        {
                                            // WSDL modified.
                                            throw new WsdlModifiedException("Could not find the message");
                                        }

                                        simpleInHeader.Name = inHeaderMessage.Name;
                                        simpleInHeader.Message = inHeaderMessage.Name;
                                        simpleOp.MessagesCollection[0].HeadersCollection.Add(simpleInHeader);

                                        // Create the in header message and put it to the Operation's messeages collection.
                                        MessagePart part = inHeaderMessage.Parts[0];
                                        if (part != null)
                                        {
                                            Message simpleInHeaderMessage = new Message();
                                            simpleInHeaderMessage.Name = inHeaderMessage.Name;
                                            simpleInHeaderMessage.Element.ElementName = part.Element.Name;
                                            simpleInHeaderMessage.Element.ElementNamespace = part.Element.Namespace;

                                            simpleOp.MessagesCollection.Add(simpleInHeaderMessage);
                                        }
                                        else
                                        {
                                            // WSDL is modified.
                                            throw new WsdlModifiedException("Could not find the message part");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // WSDL modified.
                                throw new WsdlModifiedException("Could not find the operation binding");
                            }

                            if (simpleOp.Mep == Mep.RequestResponse && opBinding.Output != null)
                            {
                                // Enumerate the message headers for the output message.
                                foreach (ServiceDescriptionFormatExtension extension in opBinding.Output.Extensions)
                                {
                                    SoapHeaderBinding outHeader = extension as SoapHeaderBinding;
                                    if (outHeader != null)
                                    {
                                        // Create the in header and add it to the headers collection.
                                        MessageHeader simpleOutHeader = new MessageHeader();
                                        FxMessage outHeaderMessage = srvDesc.Messages[outHeader.Message.Name];

                                        if (outHeaderMessage == null)
                                        {
                                            // WSDL is modified.
                                            throw new WsdlModifiedException("Could not find the message");
                                        }

                                        simpleOutHeader.Name = outHeaderMessage.Name;
                                        simpleOutHeader.Message = outHeaderMessage.Name;
                                        simpleOp.MessagesCollection[1].HeadersCollection.Add(simpleOutHeader);

                                        // Create the out header message and put it to the Operation's messeages collection.
                                        MessagePart part = outHeaderMessage.Parts[0];
                                        if (part != null)
                                        {
                                            Message simpleOutHeaderMessage = new Message();
                                            simpleOutHeaderMessage.Name = outHeaderMessage.Name;
                                            simpleOutHeaderMessage.Element.ElementName = part.Element.Name;
                                            simpleOutHeaderMessage.Element.ElementNamespace = part.Element.Namespace;

                                            simpleOp.MessagesCollection.Add(simpleOutHeaderMessage);
                                        }
                                        else
                                        {
                                            // WSDL is modified.
                                            throw new WsdlModifiedException("Could not find the message part");
                                        }
                                    }
                                }
                            }
                            else if (simpleOp.Mep == Mep.RequestResponse)
                            {
                                // WSDL modified.
                                throw new WsdlModifiedException("Could not find the operation binding");
                            }
                        }
                    }
                }
            }

            // Check for the "Generate service tags" option.
            if (srvDesc.Services.Count == 1)
            {
                simpleContract.NeedsServiceElement = true;
            }

            // Turn on the SOAP 1.2 binding if available.
            foreach (System.Web.Services.Description.Binding binding in srvDesc.Bindings)
            {
                if (binding.Extensions.Find(typeof(Soap12Binding)) != null)
                {
                    simpleContract.Bindings |= InterfaceContract.SoapBindings.Soap12;
                }
            }

            return simpleContract;
        }

        private static void CheckIfWsdlIsUsableForRoundtripping()
        {
            bool isHttpBinding = false;

            if (!ValidateWsdl(srvDesc, ref isHttpBinding))
            {
                throw new WsdlNotCompatibleForRoundTrippingException("Not a valid file for round tripping");
            }
        }

        private static void LoadWsdl(string wsdlFileName)
        {
            try
            {
                srvDesc = System.Web.Services.Description.ServiceDescription.Read(
                    wsdlFileName);
            }
            catch (Exception ex)
            {
                throw new WsdlLoadException(
                    "Could not load service description from the specified file.",
                    ex);
            }
        }

        /// <summary>
        /// Validates a specified instance of <see cref="ServiceDescription"/> class for the round tripping feature.
        /// </summary>
        /// <param name="serviceDescription">
        /// An instance of <see cref="ServiceDescription"/> class to 
        /// validate.
        /// </param>
        /// <param name="isHttpBinding">A reference to a Boolean variable. Value is this variable is set to true if the service description has Http binding.</param>
        /// <returns>
        /// A value indicating whether the specified instance of <see cref="ServiceDescription"/> 
        /// class is valid for the round tripping feature.
        /// </returns>
        private static bool ValidateWsdl(
            System.Web.Services.Description.ServiceDescription serviceDescription,
            ref bool isHttpBinding)
        {
            // Rule No 1: Service description must have atleast one schema in the types definitions.
            if (serviceDescription.Types.Schemas.Count == 0)
            {
                return false;
            }

            // Rule No 2: Service description must have only one <porttype>.
            if (serviceDescription.PortTypes.Count != 1)
            {
                return false;
            }

            // Rule No 3: Service description must have only SOAP 1.1 and/or SOAP 1.2 binding(s).
            if (!((serviceDescription.Bindings.Count == 1 && serviceDescription.Bindings[0].Extensions.Find(typeof(SoapBinding)) != null) ||
             (serviceDescription.Bindings.Count == 2 && serviceDescription.Bindings[0].Extensions.Find(typeof(SoapBinding)) != null &&
               serviceDescription.Bindings[1].Extensions.Find(typeof(Soap12Binding)) != null)))
            {
                return false;
            }

            // Rule No 4: Service description can not have more than one <service>. But it is possible 
            // not to have a <service>.
            if (serviceDescription.Services.Count > 1)
            {
                return false;
            }

            // Rule No 5: Each message must have only one <part>.
            foreach (FxMessage message in serviceDescription.Messages)
            {
                if (message.Parts.Count > 1)
                {
                    return false;
                }
            }

            // Rule No 6: For soap bindings the binding style must be 'Document' and encoding must be 'Literal'.

            // Obtain a reference to the one and only binding we have.
            System.Web.Services.Description.Binding binding = serviceDescription.Bindings[0];

            // Search for the soap binding style and return false if it is not 'Document'
            foreach (ServiceDescriptionFormatExtension extension in binding.Extensions)
            {
                SoapBinding soapBinding = extension as SoapBinding;
                if (soapBinding != null)
                {
                    if (soapBinding.Style != SoapBindingStyle.Document)
                    {
                        return false;
                    }
                }
                else if (extension is HttpBinding)
                {
                    isHttpBinding = true;
                }
            }

            // Validate the operation bindings.
            foreach (OperationBinding operationBinding in binding.Operations)
            {
                // Validate the soap binding style in soap operation binding extension.
                foreach (ServiceDescriptionFormatExtension extension in operationBinding.Extensions)
                {
                    SoapOperationBinding soapOperationBinding = extension as SoapOperationBinding;

                    if (soapOperationBinding != null)
                    {
                        if (soapOperationBinding.Style != SoapBindingStyle.Document)
                        {
                            return false;
                        }
                    }
                }

                // Validate the 'use' element in input message body and the headers.
                foreach (ServiceDescriptionFormatExtension extension in operationBinding.Input.Extensions)
                {
                    // Check for a header.
                    SoapHeaderBinding headerBinding = extension as SoapHeaderBinding;
                    if (headerBinding != null)
                    {
                        if (headerBinding.Use != SoapBindingUse.Literal)
                        {
                            return false;
                        }
                        continue;
                    }

                    // Check for the body.
                    SoapBodyBinding bodyBinding = extension as SoapBodyBinding;
                    if (bodyBinding != null)
                    {
                        if (bodyBinding.Use != SoapBindingUse.Literal)
                        {
                            return false;
                        }

                        continue;
                    }
                }

                // Validate the 'use' element in output message body and the headers.
                if (operationBinding.Output != null)
                {
                    foreach (ServiceDescriptionFormatExtension extension in operationBinding.Output.Extensions)
                    {
                        // Check for the header.
                        SoapHeaderBinding headerBinding = extension as SoapHeaderBinding;
                        if (headerBinding != null)
                        {
                            if (headerBinding.Use != SoapBindingUse.Literal)
                            {
                                return false;
                            }
                            continue;
                        }

                        // Check for the body.
                        SoapBodyBinding bodyBinding = extension as SoapBodyBinding;
                        if (bodyBinding != null)
                        {
                            if (bodyBinding.Use != SoapBindingUse.Literal)
                            {
                                return false;
                            }

                            continue;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Extracts the elements from a given schemas.
        /// </summary>
        /// <param name="schemas">Schemas to extract the elements from.</param>
        /// <param name="tns">String specifying the target namespace of the <see cref="schemas" />.</param>
        /// <returns>An instance of <see cref="SchemaElements" /> class which contains the extracted elements.</returns>
		private static List<SchemaElement> GetSchemaElements(XmlSchemas schemas, string tns)
        {
            ArrayList xmlSchemaElements = new ArrayList();
			List<SchemaElement> sElements = new List<SchemaElement>();

            foreach (XmlSchema schema in schemas)
            {
                foreach (XmlSchemaObject xmlObj in schema.Items)
                {
                    if (xmlObj is XmlSchemaAnnotated)
                    {
                        if (xmlObj is XmlSchemaElement)
                        {
                            XmlSchemaElement xse = (XmlSchemaElement)xmlObj;
                            sElements.Add(new SchemaElement(tns, xse.Name));
                        }
                    }
                }
            }

            return sElements;
        }


    }
}
