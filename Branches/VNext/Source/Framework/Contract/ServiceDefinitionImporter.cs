using System.ServiceModel.Description;
using System.Xml.Schema;
using Thinktecture.Wscf.Framework.Metadata;
using SWD = System.Web.Services.Description;

namespace Thinktecture.Wscf.Framework.Contract
{
    public class ServiceDefinitionImporter
    {
        #region "dependencies"
        private IMetadataDiscovery discoveryAgent;
        private IWsdlImporter wsdlImporter;
        private WsdlImportResult importResult;
        #endregion 

        #region "private members"
        private MetadataSet metadataSet;
        private ServiceDefinition serviceDefinition;
        private SWD.ServiceDescription rootMetadataDoc;
        #endregion

        #region "constructor(s)"
        /// <summary>
        /// Initializes a new instance of the ServiceDefinitionImporter class
        /// </summary>
        /// <param name="targetDiscoveryAgent"></param>
        /// <param name="targetWsdlImporter"></param>
        public ServiceDefinitionImporter(IMetadataDiscovery targetDiscoveryAgent, IWsdlImporter targetWsdlImporter)
        {
            this.discoveryAgent = targetDiscoveryAgent;
            this.wsdlImporter = targetWsdlImporter;
            this.serviceDefinition = new ServiceDefinition();
        }
        #endregion

        #region "Public Interface"
        /// <summary>
        /// Imports the service definition from a given endpoint or local wsdl
        /// </summary>
        /// <param name="path">path to the endpoint or local wsdl</param>
        /// <returns>A ServiceDefinition containing all the metadata.</returns>
        public ServiceDefinition Import(string path)
        {
            // Call the discoveryAgent to get the metadata from the specified path.
            this.metadataSet = this.discoveryAgent.Process(path);

            if ((this.metadataSet == null) || (this.metadataSet.MetadataSections.Count == 0))
            {
                throw new MetadataDiscoveryException("There is no metadata available at the specified endpoint");
            }
            
            // initialize the wsdl importer with the metadata set
            this.wsdlImporter.InitializeMetadataSet(metadataSet);
            this.importResult = wsdlImporter.ImportWsdl();

            // Call the private methods to build the service definition
            this.BuildServiceDefinition();
            return this.serviceDefinition;
        }

        #endregion
        #region "private methods"
        /// <summary>
        /// This is the main 'orchestrator' of the private methods that help to build up the definition
        /// </summary>
        private void BuildServiceDefinition()
        {
            this.SetRootMetadataDocument();
            this.SetMetadataInfo();
            this.SetNameAndNamespaceInfo();
            this.SetEndpointsInfo();
            this.SetBindingInfo();
            this.SetContractInfo();
            this.SetOperationsInfo();
            this.SetMessagesInfo();
            this.SetSchemaInfo();
        }

        private void SetRootMetadataDocument()
        {
            // navigate the metadatasections and find the root document
            foreach (MetadataSection section in metadataSet.MetadataSections)
            {
                SWD.ServiceDescription serviceDescription = section.Metadata as SWD.ServiceDescription;
                if (serviceDescription != null)
                {
                    // Only the root wsdl document will contain the service name and namespace
                    // non root wsdls also have a target namespace but the service collection will be empty
                    if (IsRootWsdl(serviceDescription))
                    {
                        this.rootMetadataDoc = serviceDescription;
                        return;
                    }
                }
            }
        }

        private void SetMetadataInfo()
        {
            this.serviceDefinition.MetadataSet = this.metadataSet;
        }

        private void SetNameAndNamespaceInfo()
        {
            this.serviceDefinition.Name = this.rootMetadataDoc.Services[0].Name;
            this.serviceDefinition.Namespace = this.rootMetadataDoc.TargetNamespace;
        }

        private void SetEndpointsInfo()
        {
            this.serviceDefinition.Endpoints = this.importResult.Endpoints;
        }

        private void SetBindingInfo()
        {
            this.serviceDefinition.Bindings = this.importResult.Bindings;
        }

        private void SetContractInfo()
        {
            this.serviceDefinition.Contracts = this.importResult.Contracts;
        }

        private void SetOperationsInfo()
        {
            foreach (ContractDescription contractDesc in this.serviceDefinition.Contracts)
            {
                foreach (OperationDescription od in contractDesc.Operations)
                {
                    this.serviceDefinition.Operations.Add(od);
                }
            }
        }

        private void SetMessagesInfo()
        {
            foreach (OperationDescription od in this.serviceDefinition.Operations)
            {
                foreach (MessageDescription md in od.Messages)
                {
                    this.serviceDefinition.Messages.Add(md);
                }
            }
        }

        private void SetSchemaInfo()
        {
            if (importResult.XmlSchemas != null && importResult.XmlSchemas.Count > 0)
            {
                SetSchemaInfoFromWsdlImporter();
            }
            else
            {
                SetSchemaInfoFromMetadataSet();
            }
            this.serviceDefinition.SchemaSet.Compile();
        }

        private void SetSchemaInfoFromWsdlImporter()
        {
            // Add stuf into the collection of schemas
            foreach (XmlSchema schema in importResult.XmlSchemas.Schemas())
            {
                this.serviceDefinition.Schemas.Add(schema);
            }

            // add to the schemaset
            this.serviceDefinition.SchemaSet.Add(importResult.XmlSchemas);
        }
        
        private void SetSchemaInfoFromMetadataSet()
        {
            foreach (MetadataSection section in metadataSet.MetadataSections)
            {
                if (section.Metadata is XmlSchema)
                {
                    this.serviceDefinition.Schemas.Add((XmlSchema)section.Metadata);
                    this.serviceDefinition.SchemaSet.Add((XmlSchema)section.Metadata);
                }
                else if (section.Metadata is SWD.ServiceDescription)
                {
                    SWD.ServiceDescription wsdl = (SWD.ServiceDescription)section.Metadata;
                    foreach (XmlSchema schema in wsdl.Types.Schemas)
                    {
                        if (!IsEmptySchema(schema))
                        {
                            this.serviceDefinition.Schemas.Add(schema);
                            this.serviceDefinition.SchemaSet.Add(schema);
                        }
                    }
                }
            }
        }
        
        #endregion

        #region "static helpers"
        private static bool IsRootWsdl(SWD.ServiceDescription serviceDescription)
        {
            return ((serviceDescription.Services != null) && (serviceDescription.Services.Count > 0));
        }

        private static bool IsEmptySchema(XmlSchema targetSchema)
        {
            return targetSchema.AttributeGroups.Count == 0 &&
                targetSchema.Attributes.Count == 0 &&
                targetSchema.Elements.Count == 0 &&
                targetSchema.SchemaTypes.Count == 0;
        }

        #endregion
    }
}
