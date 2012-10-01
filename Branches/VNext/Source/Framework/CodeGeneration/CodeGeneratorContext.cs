using System.CodeDom;
using System.CodeDom.Compiler;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// Encapsulates the information required by the <see cref="CodeGenerator"/>.
	/// </summary>
	public class CodeGeneratorContext : ICodeGeneratorContext
	{
		/// <summary>
		/// Gets the metadata set.
		/// </summary>
		public MetadataSet MetadataSet { get; private set; }

		/// <summary>
		/// Gets the code DOM provider.
		/// </summary>
		public CodeDomProvider CodeDomProvider { get; private set; }

		/// <summary>
		/// Gets the code compile unit.
		/// </summary>
		public CodeCompileUnit CodeCompileUnit { get; private set; }

		/// <summary>
		/// Gets the code generator options.
		/// </summary>
		public CodeGeneratorOptions CodeGeneratorOptions { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGeneratorContext"/> class.
		/// </summary>
		/// <param name="metadataSet">The metadata set.</param>
		/// <param name="codeGeneratorOptions">The code generator options.</param>
		public CodeGeneratorContext(MetadataSet metadataSet, CodeGeneratorOptions codeGeneratorOptions)
		{
			MetadataSet = metadataSet;
			CodeDomProvider = CodeDomProvider.CreateProvider(codeGeneratorOptions.CodeLanguage.ToString());
			CodeCompileUnit = new CodeCompileUnit();
			CodeGeneratorOptions = codeGeneratorOptions;
		}
	}
}