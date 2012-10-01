using System;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Exception thrown when errors are encountered during the metadata resolution process.
	/// </summary>
	[Serializable]
	public class MetadataDiscoveryException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataDiscoveryException"/> class.
		/// </summary>
		public MetadataDiscoveryException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataDiscoveryException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public MetadataDiscoveryException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataDiscoveryException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public MetadataDiscoveryException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}