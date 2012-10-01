using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml.Schema;

namespace Thinktecture.Wscf.Framework.Contract
{
    public class WsdlImportResult
    {
        public ServiceEndpointCollection Endpoints {get; set;}
        public Collection<Binding> Bindings { get; set; }
        public Collection<ContractDescription> Contracts { get; set; }
        public XmlSchemaSet XmlSchemas { get; set; }

    }
}
