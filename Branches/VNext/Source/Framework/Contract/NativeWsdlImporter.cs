using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Contract
{
    /// <summary>
    /// This class provides a wrapper over the System.ServiceModel.Description.WsdlImporter
    /// which allows WSCF to use the interface instead of the concrete type wherever applicable.
    /// </summary>
    public class NativeWsdlImporter : IWsdlImporter
    {
        private WsdlImporter wsdlImporter;

        #region constructor(s)
        #endregion

        #region IWsdlImporter Members

        public void InitializeMetadataSet(MetadataSet targetMetadataSet)
        {
            this.wsdlImporter = new WsdlImporter(targetMetadataSet);            
        }

        public WsdlImportResult ImportWsdl()
        {
            WsdlImportResult result = new WsdlImportResult();
            result.Endpoints = this.wsdlImporter.ImportAllEndpoints();
            result.Bindings = this.wsdlImporter.ImportAllBindings();
            result.Contracts = this.wsdlImporter.ImportAllContracts();
            result.XmlSchemas = this.wsdlImporter.XmlSchemas;
            return result;
        }

        #endregion
    }
}
