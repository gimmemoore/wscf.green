namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// The file types that represent metadata.
	/// </summary>
	internal enum MetadataFileType
	{
		/// <summary>
		/// A file that is not XML.
		/// </summary>
		NonXml,
		/// <summary>
		/// A WSDL file.
		/// </summary>
		Wsdl,
		/// <summary>
		/// An XSD file.
		/// </summary>
		Xsd,
		/// <summary>
		/// A WS-Policy file.
		/// </summary>
		Policy,
		/// <summary>
		/// An Endpoint Reference file.
		/// </summary>
		Epr,
		/// <summary>
		/// An unknown XML file.
		/// </summary>
		UnknownXml
	}
}