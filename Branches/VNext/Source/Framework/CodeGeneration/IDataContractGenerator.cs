using System.CodeDom;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// Generates a <see cref="CodeCompileUnit"/> of data contracts from a metadata set.
	/// </summary>
	public interface IDataContractGenerator
	{
		/// <summary>
		/// Generates the <see cref="CodeCompileUnit"/> based on the provided context.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>A <see cref="CodeCompileUnit"/> containing the generated code.</returns>
		CodeCompileUnit GenerateCode(ICodeGeneratorContext codeGeneratorContext);
	}
}