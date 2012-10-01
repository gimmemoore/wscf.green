using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="ServiceContractGenerationOptions"/> instances.
	/// </summary>
	public interface IServiceContractGenerationOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorOptions">The code generator options.</param>
		/// <returns>
		/// A new <see cref="ServiceContractGenerationOptions"/> instance.
		/// </returns>
		ServiceContractGenerationOptions Build(CodeGeneratorOptions codeGeneratorOptions);
	}
}