using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A factory for creating <see cref="ServiceContractGenerator"/> instances.
	/// </summary>
	public interface IServiceContractGeneratorFactory
	{
		/// <summary>
		/// Gets a new <see cref="ServiceContractGenerator"/> instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="ServiceContractGenerator"/> instance.
		/// </returns>
		ServiceContractGenerator GetServiceContractGenerator(ICodeGeneratorContext codeGeneratorContext);
	}
}