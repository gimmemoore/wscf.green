using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A factory for creating <see cref="WsdlImporter"/> instances.
	/// </summary>
	public interface IWsdlImporterFactory
	{
		/// <summary>
		/// Gets a new <see cref="WsdlImporter"/> instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="WsdlImporter"/> instance.
		/// </returns>
		WsdlImporter GetWsdlImporter(ICodeGeneratorContext codeGeneratorContext);
	}
}