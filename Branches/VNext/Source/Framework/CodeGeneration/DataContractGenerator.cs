using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml.Schema;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// Generates a <see cref="CodeCompileUnit"/> of data contracts from a metadata set.
	/// </summary>
	public class DataContractGenerator : IDataContractGenerator
	{
		private readonly IXsdDataContractImporterBuilder xsdDataContractImporterBuilder;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataContractGenerator"/> class.
		/// </summary>
		/// <param name="xsdDataContractImporterBuilder">The XSD data contract importer builder.</param>
		public DataContractGenerator(IXsdDataContractImporterBuilder xsdDataContractImporterBuilder)
		{
			this.xsdDataContractImporterBuilder = xsdDataContractImporterBuilder;
		}

		/// <summary>
		/// Generates the <see cref="CodeCompileUnit"/> based on the provide context.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>A <see cref="CodeCompileUnit"/> containing the generated code.</returns>
		public CodeCompileUnit GenerateCode(ICodeGeneratorContext codeGeneratorContext)
		{
			XsdDataContractImporter xsdDataContractImporter = xsdDataContractImporterBuilder.Build(codeGeneratorContext);
			XmlSchemaSet xmlSchemaSet = ExtractXmlSchemas(codeGeneratorContext.MetadataSet.MetadataSections);
			xsdDataContractImporter.Import(xmlSchemaSet);

			return xsdDataContractImporter.CodeCompileUnit;
		}

		private static XmlSchemaSet ExtractXmlSchemas(IEnumerable<MetadataSection> metadataSections)
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet {XmlResolver = null};
			foreach (MetadataSection metadataSection in metadataSections)
			{
				ServiceDescription serviceDescription = metadataSection.Metadata as ServiceDescription;
				if (((serviceDescription != null) && (serviceDescription.Types != null)) && (serviceDescription.Types.Schemas != null))
				{
					foreach (XmlSchema schema in serviceDescription.Types.Schemas)
					{
						xmlSchemaSet.Add(schema);
					}
					continue;
				}
				XmlSchema xmlSchema = metadataSection.Metadata as XmlSchema;
				if (xmlSchema != null)
				{
					xmlSchemaSet.Add(xmlSchema);
				}
			}
			return xmlSchemaSet;
		}
	}
}