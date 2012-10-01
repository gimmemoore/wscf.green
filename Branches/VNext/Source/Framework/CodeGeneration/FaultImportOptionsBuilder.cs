using System.ServiceModel;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="FaultImportOptions"/> instances.
	/// </summary>
	public class FaultImportOptionsBuilder : IFaultImportOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <returns>A new <see cref="FaultImportOptions"/> instance.</returns>
		public FaultImportOptions Build()
		{
			return new FaultImportOptions {UseMessageFormat = true};
		}
	}
}