using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Text;
using Thinktecture.Wscf.Framework.Properties;

namespace Thinktecture.Wscf.Framework.Tests.Helpers
{
	internal static class TestMetadata
	{
		private static readonly MetadataSet metadataSet;

		static TestMetadata()
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(MetadataSet));
			using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(Resources.RestaurantMetadata)))
			{
				metadataSet = (MetadataSet)serializer.ReadObject(stream);
			}
		}

		internal static MetadataSet MetadataSet
		{
			get { return metadataSet; }
		}
	}
}
