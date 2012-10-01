namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// The type of serializer that will be targeted.
	/// </summary>
	public enum SerializerMode
	{
		/// <summary>
		/// The serializer will be selected automatically.
		/// </summary>
		Auto,

		/// <summary>
		/// The Data Contract serializer will be used.
		/// </summary>
		DataContractSerializer,

		/// <summary>
		/// The XML serializer will be used.
		/// </summary>
		XmlSerializer
	}
}