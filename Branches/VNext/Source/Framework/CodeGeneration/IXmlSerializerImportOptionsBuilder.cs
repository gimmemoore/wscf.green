using System.ServiceModel.Channels;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="XmlSerializerImportOptions"/> instances.
	/// </summary>
	public interface IXmlSerializerImportOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="XmlSerializerImportOptions"/> instance.
		/// </returns>
		XmlSerializerImportOptions Build(ICodeGeneratorContext codeGeneratorContext);
	}
}