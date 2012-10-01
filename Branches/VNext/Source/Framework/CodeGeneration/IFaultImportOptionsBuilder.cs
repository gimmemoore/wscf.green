using System.ServiceModel;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="FaultImportOptions"/> instances.
	/// </summary>
	public interface IFaultImportOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <returns>A new <see cref="FaultImportOptions"/> instance.</returns>
		FaultImportOptions Build();
	}
}