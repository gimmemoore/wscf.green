using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="XsdDataContractImporter"/> instances.
	/// </summary>
	public class XsdDataContractImporterBuilder : IXsdDataContractImporterBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="XsdDataContractImporter"/> instance.
		/// </returns>
		public XsdDataContractImporter Build(ICodeGeneratorContext codeGeneratorContext)
		{
			CodeGeneratorOptions codeGeneratorOptions = codeGeneratorContext.CodeGeneratorOptions;
			CodeDomProvider codeDomProvider = codeGeneratorContext.CodeDomProvider;
			CodeCompileUnit codeCompileUnit = codeGeneratorContext.CodeCompileUnit;

			ImportOptions importOptions = new ImportOptions
			{
				GenerateSerializable = codeGeneratorOptions.SerializableAttribute,
				GenerateInternal = codeGeneratorOptions.InternalTypes,
				ImportXmlType = codeGeneratorOptions.ImportXmlTypes,
				EnableDataBinding = codeGeneratorOptions.EnableDataBinding,
				CodeProvider = codeDomProvider
			};

			foreach (KeyValuePair<string, string> mapping in codeGeneratorOptions.NamespaceMappings)
			{
				importOptions.Namespaces.Add(mapping.Key, mapping.Value);
			}

			return new XsdDataContractImporter(codeCompileUnit) {Options = importOptions};
		}
	}
}