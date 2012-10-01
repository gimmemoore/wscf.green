using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Schema;

namespace Thinktecture.Wscf.Framework.Contract
{
    public class ServiceDefinition
    {
        /// <summary>
        /// The name of the service.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Namespace of the service
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// List of all endpoints
        /// </summary>
        public ServiceEndpointCollection Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the List of all Bindings.
        /// </summary>
        public Collection<Binding> Bindings { get; set; }

        /// <summary>
        /// Gets or sets the List of all Behaviors.
        /// </summary>
        public List<IServiceBehavior> Behaviors { get; set; }

        /// <summary>
        /// List of all contracts
        /// </summary>
        public Collection<ContractDescription> Contracts { get; set; }

        /// <summary>
        /// List of all operations
        /// </summary>
        public List<OperationDescription> Operations { get; set; }

        /// <summary>
        /// List of all messages
        /// </summary>
        public List<MessageDescription> Messages { get; set; }

        /// <summary>
        /// List of all schemas
        /// </summary>
        public Collection<XmlSchema> Schemas { get; set; }

        public XmlSchemaSet SchemaSet { get; set; }

        /// <summary>
        /// Metadataset
        /// </summary>
        public MetadataSet MetadataSet { get; set; }


        public ServiceDefinition()
        {
            this.Bindings = new Collection<Binding>();
            this.Behaviors = new List<IServiceBehavior>();
            this.Contracts = new Collection<ContractDescription>();
            this.Operations = new List<OperationDescription>();
            this.Messages = new List<MessageDescription>();
            this.Schemas = new Collection<XmlSchema>();
            this.SchemaSet = new XmlSchemaSet();
        }

        public MessageDescription FindMessage(string messageName)
        {
            MessageDescription result = null;

            foreach (MessageDescription md in this.Messages)
            {
                // In the case of input we also have to cater to wrapper name
                if (md.Direction == MessageDirection.Input)
                {
                    if ((!String.IsNullOrEmpty(md.Body.WrapperName) && md.Body.WrapperName.Equals(messageName))
                        || (md.Body.Parts != null && md.Body.Parts[0].Name.Equals(messageName)))
                    {
                        result = md;
                        break;
                    }
                }
                else
                {
                    if ((!String.IsNullOrEmpty(md.Body.WrapperName) && md.Body.WrapperName.Equals(messageName))
                        || (md.Body.ReturnValue != null && md.Body.ReturnValue.Name.Equals(messageName)))
                    {
                        result = md;
                        break;
                    }

                }
            }

            return result;

        }
        public static MessageDescription FindOperationMessage(OperationDescription sourceOp, MessageDirection targetMsgDirection)
        {
            MessageDescription result = null;
            switch (targetMsgDirection)
            {
                case MessageDirection.Input:
                    foreach (MessageDescription md in sourceOp.Messages)
                    {
                        if (md.Direction == MessageDirection.Input)
                        {
                            result = md;
                            break;
                        }
                    }
                    break;
                case MessageDirection.Output:
                    foreach (MessageDescription md in sourceOp.Messages)
                    {
                        if (md.Direction == MessageDirection.Output)
                        {
                            result = md;
                            break;
                        }
                    }

                    break;
                default:
                    result = null;
                    break;
            }

            return result;


        }
        public static string GetMessageName(MessageDescription sourceMsg)
        {
            string msgName;

            if (!String.IsNullOrEmpty(sourceMsg.Body.WrapperName) && !String.IsNullOrEmpty(sourceMsg.Body.WrapperNamespace))
            {
                msgName = sourceMsg.Body.WrapperName;
            }
            else
            {
                if (sourceMsg.Direction == MessageDirection.Input)
                {
                    msgName = sourceMsg.Body.Parts[0].Name;
                }
                else
                {
                    msgName = sourceMsg.Body.ReturnValue.Name;
                }
            }

            return msgName;
        }
        public static string GetMessageNamespace(MessageDescription sourceMsg)
        {
            string msgNamespace;

            if (!String.IsNullOrEmpty(sourceMsg.Body.WrapperName) && !String.IsNullOrEmpty(sourceMsg.Body.WrapperNamespace))
            {
                msgNamespace = sourceMsg.Body.WrapperNamespace;
            }
            else
            {
                if (sourceMsg.Direction == MessageDirection.Input)
                {
                    msgNamespace = sourceMsg.Body.Parts[0].Namespace;
                }
                else
                {
                    msgNamespace = sourceMsg.Body.ReturnValue.Namespace;
                }
            }

            return msgNamespace;
        }
        public static XmlQualifiedName GetMessageQName(MessageDescription msg)
        {
            XmlQualifiedName qName;

            // it doesnt matter if its an input message or output message. 
            // if the wrapper name is present, then we need to use that 
            // if not, then we take the name from different places
            if (!String.IsNullOrEmpty(msg.Body.WrapperName) && !String.IsNullOrEmpty(msg.Body.WrapperNamespace))
            {
                qName = new XmlQualifiedName(msg.Body.WrapperName, msg.Body.WrapperNamespace);
            }
            else
            {
                if (msg.Direction == MessageDirection.Input)
                {
                    qName = new XmlQualifiedName(msg.Body.Parts[0].Name, msg.Body.Parts[0].Namespace);
                }
                else
                {
                    qName = new XmlQualifiedName(msg.Body.ReturnValue.Name, msg.Body.ReturnValue.Namespace);
                }
            }
            return qName;
        }
    }
}
