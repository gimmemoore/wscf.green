using System.Runtime.Serialization;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="XsdDataContractImporter"/> instances.
	/// </summary>
	public interface IXsdDataContractImporterBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="XsdDataContractImporter"/> instance.
		/// </returns>
		XsdDataContractImporter Build(ICodeGeneratorContext codeGeneratorContext);
	}
}