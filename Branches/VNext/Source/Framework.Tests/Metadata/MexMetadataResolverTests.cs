using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.Metadata
{
	[TestFixture]
	public class MexMetadataResolverTests
	{
		private TestHost host;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			host = new TestHost();
			host.AddHttpEndpoints();
			host.AddNamePipeEndpoints();
			host.AddTcpEndpoints();
			host.Open();

			TestFiles.WriteFiles();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			host.Dispose();

			TestFiles.DeleteFiles();
		}

		[Test]
		public void Resolve_InvalidUriScheme_ExceptionThrown()
		{
			Uri invalidSchemeUri = new Uri("foo://localhost/RestaurantService");
			IMexMetadataResolver resolver = new MexMetadataResolver();
			Assert.Throws<MexMetadataDiscoveryException>(() => resolver.Resolve(invalidSchemeUri));
		}

		[Test]
		public void Resolve_NamedPipeMexEndpoint_MetadataDiscovered()
		{
			ResolveMetadataAndAssert(new Uri(TestHost.NamedPipeMexAddress));
		}

		[Test]
		public void Resolve_TcpMexEndpoint_MetadataDiscovered()
		{
			ResolveMetadataAndAssert(new Uri(TestHost.TcpMexAddress));
		}

		[Test]
		public void Resolve_HttpMexEndpoint_MetadataDiscovered()
		{
			ResolveMetadataAndAssert(new Uri(TestHost.HttpMexAddress));
		}

		[Test]
		public void Resolve_HttpGetEndpoint_MetadataDiscovered()
		{
			ResolveMetadataAndAssert(new Uri(TestHost.HttpGetAddress));
		}

		private static void ResolveMetadataAndAssert(Uri mexAddress)
		{
			IMexMetadataResolver resolver = new MexMetadataResolver();
			IEnumerable<MetadataSection> metadataSections = resolver.Resolve(mexAddress);
				
			Assert.That(metadataSections, Has.Count.EqualTo(4));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.WsdlNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.TempNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.DataNamespace), Is.EqualTo(1));
			Assert.That(metadataSections.Count(m => m.Identifier == TestNamespaces.MessageNamespace), Is.EqualTo(1));
		}
	}
}
