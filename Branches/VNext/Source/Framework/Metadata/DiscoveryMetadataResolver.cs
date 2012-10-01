using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Schema;

using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Resolves metadata using the <see cref="DiscoveryClientProtocol"/> class.
	/// This class supports processing of files from the local file system.
	/// </summary>
	public static class DiscoveryMetadataResolver
	{
		/// <summary>
		/// Resolves metadata from the specified URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>A list of metadata sections.</returns>
		public static IEnumerable<MetadataSection> Resolve(string url)
		{
			MetadataSet metadataSet = new MetadataSet();
			DiscoveryClientProtocol discoveryClient = new DiscoveryClientProtocol
			{
				UseDefaultCredentials = true,
				AllowAutoRedirect = true
			};
			discoveryClient.DiscoverAny(url);
			discoveryClient.ResolveAll();
			foreach (object document in discoveryClient.Documents.Values)
			{
				AddDocumentToSet(metadataSet, document);
			}

			return metadataSet.MetadataSections;
		}

		private static void AddDocumentToSet(MetadataSet metadataSet, object document)
		{
			ServiceDescription serviceDescription = document as ServiceDescription;
			XmlSchema schema = document as XmlSchema;
			XmlElement policy = document as XmlElement;

			if (serviceDescription != null)
			{
				metadataSet.MetadataSections.Add(MetadataSection.CreateFromServiceDescription(serviceDescription));
			}
			else if (schema != null)
			{
				metadataSet.MetadataSections.Add(MetadataSection.CreateFromSchema(schema));
			}
			else if ((policy != null) && IsPolicyElement(policy))
			{
				metadataSet.MetadataSections.Add(MetadataSection.CreateFromPolicy(policy, null));
			}
			else
			{
				MetadataSection item = new MetadataSection {Metadata = document};
				metadataSet.MetadataSections.Add(item);
			}
		}

		private static bool IsPolicyElement(XmlNode policy)
		{
			return (policy.NamespaceURI == "http://schemas.xmlsoap.org/ws/2004/09/policy"
				|| policy.NamespaceURI == "http://www.w3.org/ns/ws-policy") && policy.LocalName == "Policy";
		}
	}
}