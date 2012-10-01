using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.Metadata
{
	[TestFixture]
	public class XmlMetadataResolverTests
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
		public void Resolve_WsdlFile_MetadataImported()
		{
			string wsdlFileName = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);
			FileInfo wsdlFileInfo = new FileInfo(wsdlFileName);
			IXmlMetadataResolver resolver = new XmlMetadataResolver();
			IEnumerable<MetadataSection> metadataSections = resolver.Resolve(wsdlFileInfo);
			
			Assert.That(metadataSections, Has.Count.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.WsdlNamespace), Is.EqualTo(1));
		}
		
		[Test]
		public void Resolve_XsdFile_MetadataImported()
		{
			string xsdFileName = TestFiles.GetFilePath(TestFiles.RestaurantDataXsdFileName);
			FileInfo xsdFileInfo = new FileInfo(xsdFileName);
			IXmlMetadataResolver resolver = new XmlMetadataResolver();
			IEnumerable<MetadataSection> metadataSections = resolver.Resolve(xsdFileInfo);

			Assert.That(metadataSections, Has.Count.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.DataNamespace), Is.EqualTo(1));
		}
	}
}
