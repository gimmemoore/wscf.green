using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Resolves metadata from the local file system.
	/// </summary>
	public interface IXmlMetadataResolver
	{
		/// <summary>
		/// Resolves metadata from the specified file.
		/// </summary>
		/// <param name="fileInfo">The file information.</param>
		/// <returns>A list of metadata sections.</returns>
		IEnumerable<MetadataSection> Resolve(FileInfo fileInfo);
	}
}