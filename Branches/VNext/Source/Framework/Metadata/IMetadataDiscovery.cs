using System.Collections.Generic;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Provides support for the discovery of metadata from local and remote sources.
	/// </summary>
	public interface IMetadataDiscovery
	{
		/// <summary>
		/// Discovers any known types of metadata in the specified paths.
		/// </summary>
		/// <param name="paths">The paths to search.</param>
		/// <returns>A complete set of all discovered metadata.</returns>
		MetadataSet Process(IEnumerable<string> paths);

		/// <summary>
		/// Discovers any known types of metadata in the specified paths.
		/// </summary>
		/// <param name="paths">The paths to search.</param>
		/// <returns>A complete set of all discovered metadata.</returns>
		MetadataSet Process(params string[] paths);
	}
}