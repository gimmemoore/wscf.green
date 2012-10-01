using System;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Exception thrown when MEX or HTTP GET metadata cannot be retrieved.
	/// </summary>
	[Serializable]
	public class MexMetadataDiscoveryException : MetadataDiscoveryException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MexMetadataDiscoveryException"/> class.
		/// </summary>
		/// <param name="mexException">The MEX exception.</param>
		/// <param name="httpGetException">The HTTP GET exception.</param>
		/// <param name="serviceUri">The service URI.</param>
		public MexMetadataDiscoveryException(MetadataDiscoveryException mexException, MetadataDiscoveryException httpGetException, Uri serviceUri)
			: base(string.Format("The metadata from '{0}' could not be obtained.", serviceUri.AbsoluteUri))
		{
			MexException = mexException;
			HttpGetException = httpGetException;
			ServiceUri = serviceUri;
		}

		/// <summary>
		/// Gets the service URI.
		/// </summary>
		public Uri ServiceUri { get; private set; }

		/// <summary>
		/// Gets HTTP GET exception.
		/// </summary>
		public MetadataDiscoveryException HttpGetException { get; private set; }

		/// <summary>
		/// Gets the MEX exception.
		/// </summary>
		public MetadataDiscoveryException MexException { get; private set; }
	}
}