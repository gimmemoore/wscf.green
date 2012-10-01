using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Resolves metadata from MEX and HTTP GET endpoints.
	/// </summary>
	public interface IMexMetadataResolver
	{
		/// <summary>
		/// Resolves metadata from the specified endpoint reference.
		/// </summary>
		/// <param name="endpointReference">The endpoint reference.</param>
		/// <returns>A list of metadata sections.</returns>
		IEnumerable<MetadataSection> Resolve(EndpointAddress endpointReference);

		/// <summary>
		/// Resolves metadata from the specified service URI.
		/// </summary>
		/// <param name="serviceUri">The service URI.</param>
		/// <returns>A list of metadata sections.</returns>
		IEnumerable<MetadataSection> Resolve(Uri serviceUri);
	}
}