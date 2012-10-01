using System;
using System.IO;

using Moq;
using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.Metadata
{
	[TestFixture]
	public class MetadataDiscoveryTests
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
		public void Resolve_SingleExplicitFile_Discovered()
		{
			string wsdlFile = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);

			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(wsdlFile);

			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == wsdlFile)), Times.Once());
		}

		[Test]
		public void Resolve_MultipleExplicitFiles_AllDiscovered()
		{
			string wsdlFile = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);
			string dataFile = TestFiles.GetFilePath(TestFiles.RestaurantDataXsdFileName);
			string headerFile = TestFiles.GetFilePath(TestFiles.RestaurantHeaderDataXsdFileName);
			string messageFile = TestFiles.GetFilePath(TestFiles.RestaurantMessagesXsdFileName);
			
			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(wsdlFile, dataFile, headerFile, messageFile);

			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == wsdlFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == dataFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == headerFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == messageFile)), Times.Once());
		}

		[Test]
		public void Resolve_MultipleImplicitFiles_AllDiscovered()
		{
			string xsdFiles = Path.Combine(TestFiles.TempFolder, "*.xsd");
			string wsdlFiles = Path.Combine(TestFiles.TempFolder, "*.wsdl");

			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(xsdFiles, wsdlFiles);
		}

		[Test]
		public void Resolve_NamedPipeMexEndpoint_MetadataDiscovered()
		{
			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(TestHost.NamedPipeMexAddress);

			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.NamedPipeMexAddress)), Times.Once());
		}

		[Test]
		public void Resolve_TcpMexEndpoint_MetadataDiscovered()
		{
			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(TestHost.TcpMexAddress);

			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.TcpMexAddress)), Times.Once());
		}

		[Test]
		public void Resolve_HttpMexEndpoint_MetadataDiscovered()
		{
			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(TestHost.HttpMexAddress);

			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.HttpMexAddress)), Times.Once());
		}

		[Test]
		public void Resolve_HttpGetEndpoint_MetadataDiscovered()
		{
			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(TestHost.HttpGetAddress);

			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.HttpGetAddress)), Times.Once());
		}

		[Test]
		public void Resolve_MultipleFilesAndEndpoints_AllDiscovered()
		{
			string wsdlFile = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);
			string dataFile = TestFiles.GetFilePath(TestFiles.RestaurantDataXsdFileName);
			string headerFile = TestFiles.GetFilePath(TestFiles.RestaurantHeaderDataXsdFileName);
			string messageFile = TestFiles.GetFilePath(TestFiles.RestaurantMessagesXsdFileName);

			Mock<IMexMetadataResolver> mexResolver = new Mock<IMexMetadataResolver>();
			Mock<IXmlMetadataResolver> xmlResolver = new Mock<IXmlMetadataResolver>();

			IMetadataDiscovery metadataDiscovery = new MetadataDiscovery(mexResolver.Object, xmlResolver.Object);
			metadataDiscovery.Process(wsdlFile, dataFile, headerFile, messageFile,
				TestHost.NamedPipeMexAddress,
				TestHost.TcpMexAddress,
				TestHost.HttpMexAddress,
				TestHost.HttpGetAddress);

			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == wsdlFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == dataFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == headerFile)), Times.Once());
			xmlResolver.Verify(mock => mock.Resolve(It.Is<FileInfo>(v => v.FullName == messageFile)), Times.Once());

			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.NamedPipeMexAddress)), Times.Once());
			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.TcpMexAddress)), Times.Once());
			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.HttpMexAddress)), Times.Once());
			mexResolver.Verify(mock => mock.Resolve(It.Is<Uri>(v => v.AbsoluteUri == TestHost.HttpGetAddress)), Times.Once());
		}
	}
}
