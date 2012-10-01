using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Contract
{
    /// <summary>
    /// Defines an interface over a family of classes that provide WSDL Import functionality.
    /// </summary>
    public interface IWsdlImporter
    {
        void InitializeMetadataSet(MetadataSet targetMetadataSet);
        WsdlImportResult ImportWsdl();
    }
}
