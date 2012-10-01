using System.CodeDom;
using System.CodeDom.Compiler;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// Encapsulates the information required by the <see cref="CodeGenerator"/>.
	/// </summary>
	public interface ICodeGeneratorContext
	{
		/// <summary>
		/// Gets the metadata set.
		/// </summary>
		MetadataSet MetadataSet { get; }

		/// <summary>
		/// Gets the code DOM provider.
		/// </summary>
		CodeDomProvider CodeDomProvider { get; }

		/// <summary>
		/// Gets the code compile unit.
		/// </summary>
		CodeCompileUnit CodeCompileUnit { get; }

		/// <summary>
		/// Gets the code generator options.
		/// </summary>
		CodeGeneratorOptions CodeGeneratorOptions { get; }
	}
}