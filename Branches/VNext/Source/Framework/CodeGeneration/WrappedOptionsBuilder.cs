using System.ServiceModel.Channels;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="WrappedOptions"/> instances.
	/// </summary>
	public class WrappedOptionsBuilder : IWrappedOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorOptions">The code generator options.</param>
		/// <returns>
		/// A new <see cref="WrappedOptions"/> instance.
		/// </returns>
		public WrappedOptions Build(CodeGeneratorOptions codeGeneratorOptions)
		{
			return new WrappedOptions {WrappedFlag = codeGeneratorOptions.Wrapped};
		}
	}
}