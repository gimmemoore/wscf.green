using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.Data.Design;
using System.ServiceModel.Channels;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="XmlSerializerImportOptions"/> instances.
	/// </summary>
	public class XmlSerializerImportOptionsBuilder : IXmlSerializerImportOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="XmlSerializerImportOptions"/> instance.
		/// </returns>
		public XmlSerializerImportOptions Build(ICodeGeneratorContext codeGeneratorContext)
		{
			CodeGeneratorOptions codeGeneratorOptions = codeGeneratorContext.CodeGeneratorOptions;
			CodeDomProvider codeDomProvider = codeGeneratorContext.CodeDomProvider;
			CodeCompileUnit codeCompileUnit = codeGeneratorContext.CodeCompileUnit;

			WebReferenceOptions webReferenceOptions = new WebReferenceOptions
			{
				CodeGenerationOptions = CodeGenerationOptions.None
			};

			if (codeGeneratorOptions.OrderMembers)
			{
				webReferenceOptions.CodeGenerationOptions |= CodeGenerationOptions.GenerateOrder;
			}
			if (codeGeneratorOptions.GenerateProperties)
			{
				webReferenceOptions.CodeGenerationOptions |= CodeGenerationOptions.GenerateProperties;
			}
			if (codeGeneratorOptions.EnableDataBinding)
			{
				webReferenceOptions.CodeGenerationOptions |= CodeGenerationOptions.EnableDataBinding;
			}

			string typedDataSetExtension = (codeGeneratorOptions.TargetFrameworkVersion == TargetFrameworkVersion.Version35)
				? typeof(TypedDataSetSchemaImporterExtensionFx35).AssemblyQualifiedName
				: typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName;
			webReferenceOptions.SchemaImporterExtensions.Add(typedDataSetExtension);
			webReferenceOptions.SchemaImporterExtensions.Add(typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);

			XmlSerializerImportOptions xmlSerializerImportOptions = new XmlSerializerImportOptions(codeCompileUnit)
			{
				WebReferenceOptions = webReferenceOptions,
				CodeProvider = codeDomProvider
			};

			// TODO:Alex:Prevent the need to keep all types in the same namespaces for certain code decorators.
			string namespaceMapping;
			codeGeneratorOptions.NamespaceMappings.TryGetValue("*", out namespaceMapping);
			xmlSerializerImportOptions.ClrNamespace = namespaceMapping;

			return xmlSerializerImportOptions;
		}
	}
}