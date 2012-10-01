using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.Metadata
{
	[TestFixture]
	public class DiscoveryMetadataResolverTests
	{
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			TestFiles.WriteFiles();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			TestFiles.DeleteFiles();
		}

		[Test]
		public void Resolve_WsdlFile_ImportsDiscovered()
		{
			string wsdlFileName = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);
			IEnumerable<MetadataSection> metadataSections = DiscoveryMetadataResolver.Resolve(wsdlFileName);

			Assert.That(metadataSections, Has.Count.EqualTo(3));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.WsdlNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.DataNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.MessageNamespace), Is.EqualTo(1));
		}

		[Test]
		public void Resolve_XsdFile_ImportsDiscovered()
		{
			string xsdFileName = TestFiles.GetFilePath(TestFiles.RestaurantMessagesXsdFileName);
			IEnumerable<MetadataSection> metadataSections = DiscoveryMetadataResolver.Resolve(xsdFileName);

			Assert.That(metadataSections, Has.Count.EqualTo(2));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.DataNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.MessageNamespace), Is.EqualTo(1));
		}
	}
}