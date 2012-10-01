using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Schema;

using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Resolves metadata from the local file system.
	/// </summary>
	public class XmlMetadataResolver : IXmlMetadataResolver
	{
		private readonly IMexMetadataResolver mexMetadataResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMetadataResolver"/> class.
		/// </summary>
		public XmlMetadataResolver() : this(new MexMetadataResolver())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMetadataResolver"/> class.
		/// </summary>
		/// <param name="mexMetadataResolver">The MEX metadata resolver.</param>
		internal XmlMetadataResolver(IMexMetadataResolver mexMetadataResolver)
		{
			this.mexMetadataResolver = mexMetadataResolver;
		}

		/// <summary>
		/// Resolves metadata from the specified file.
		/// </summary>
		/// <param name="fileInfo">The file information.</param>
		/// <returns>A list of metadata sections.</returns>
		public IEnumerable<MetadataSection> Resolve(FileInfo fileInfo)
		{
			using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (XmlReader reader = XmlReader.Create(fileStream))
				{
					MetadataFileType type = DetermineFileType(reader);
					switch (type)
					{
						case MetadataFileType.Wsdl:
							return LoadAsWsdl(reader, fileInfo.FullName);

						case MetadataFileType.Xsd:
							return LoadAsXmlSchema(reader, fileInfo.FullName);

						case MetadataFileType.Policy:
							return LoadAsPolicy(reader, fileInfo.FullName);

						case MetadataFileType.Epr:
							return LoadAsEpr(reader, fileInfo.FullName);

						case MetadataFileType.UnknownXml:
							return LoadAsUnknownXml(reader, fileInfo.FullName);
					}
					throw new MetadataDiscoveryException("The file '{0}' does not appear to be an XML metadata file.");
				}
			}
		}

		private static MetadataFileType DetermineFileType(XmlReader reader)
		{
			try
			{
				if (reader.IsStartElement("definitions", "http://schemas.xmlsoap.org/wsdl/"))
				{
					return MetadataFileType.Wsdl;
				}
				if (reader.IsStartElement("schema", "http://www.w3.org/2001/XMLSchema"))
				{
					return MetadataFileType.Xsd;
				}
				if (reader.IsStartElement("Policy", "http://schemas.xmlsoap.org/ws/2004/09/policy") || reader.IsStartElement("Policy", "http://www.w3.org/ns/ws-policy"))
				{
					return MetadataFileType.Policy;
				}
				if (reader.IsStartElement("EndpointReference", "http://schemas.xmlsoap.org/ws/2004/08/addressing"))
				{
					return MetadataFileType.Epr;
				}
				return MetadataFileType.UnknownXml;
			}
			catch (XmlException)
			{
				return MetadataFileType.NonXml;
			}
		}

		private static IEnumerable<MetadataSection> LoadAsWsdl(XmlReader reader, string path)
		{
			ServiceDescription description;
			try
			{
				description = ServiceDescription.Read(reader);
			}
			catch (Exception exception)
			{
				if (exception.IsFatal()) throw;

				string message = string.Format("WSDL Parsing Error while reading: '{0}'. Verify that the XML is both well-formed and valid.", path);
				throw new MetadataDiscoveryException(message);
			}
			return new List<MetadataSection> {MetadataSection.CreateFromServiceDescription(description)};
		}

		private static IEnumerable<MetadataSection> LoadAsXmlSchema(XmlReader reader, string path)
		{
			XmlSchema schema;
			try
			{
				schema = XmlSchema.Read(reader, null);
			}
			catch (Exception exception)
			{
				if (exception.IsFatal()) throw;

				string message = string.Format("XML Schema Parsing Error while reading: '{0}'. Verify that the XML is both well-formed and valid.", path);
				throw new MetadataDiscoveryException(message);
			}
			return new List<MetadataSection> {MetadataSection.CreateFromSchema(schema)};
		}

		private static IEnumerable<MetadataSection> LoadAsPolicy(XmlReader reader, string path)
		{
			XmlDocument document;
			try
			{
				document = new XmlDocument();
				document.Load(reader);
			}
			catch (Exception exception)
			{
				if (exception.IsFatal()) throw;

				string message = string.Format("WS-Policy Parsing Error while reading: '{0}'. Verify that the XML is both well-formed and valid.", path);
				throw new MetadataDiscoveryException(message);
			}
			XmlElement documentElement = document.DocumentElement;
			return (documentElement != null) 
				? new List<MetadataSection> {MetadataSection.CreateFromPolicy(documentElement, null)} 
				: Enumerable.Empty<MetadataSection>();
		}

		private IEnumerable<MetadataSection> LoadAsEpr(XmlReader reader, string path)
		{
			EndpointAddress address;
			try
			{
				address = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
			}
			catch (Exception exception)
			{
				if (exception.IsFatal()) throw;

				string message = string.Format("Cannot read Endpoint Reference from: '{0}'. Verify that the XML is both well-formed and valid.", path);
				throw new MetadataDiscoveryException(message);
			}
			return mexMetadataResolver.Resolve(address);
		}

		private static IEnumerable<MetadataSection> LoadAsUnknownXml(XmlReader reader, string path)
		{
			XmlDocument document;
			try
			{
				document = new XmlDocument();
				document.Load(reader);
			}
			catch (Exception exception)
			{
				if (exception.IsFatal()) throw;

				string message = string.Format("Unknown XML Parsing Error while reading: '{0}'. Verify that the XML is both well-formed and valid.", path);
				throw new MetadataDiscoveryException(message);
			}
			XmlElement documentElement = document.DocumentElement;
			return new List<MetadataSection> {new MetadataSection(null, null, documentElement)};
		}
	}
}
