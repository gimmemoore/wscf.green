using NUnit.Framework;

namespace Thinktecture.Wscf.Framework.Contract
{
    using Thinktecture.Wscf.Framework.Tests.Helpers;

    [TestFixture]
    public class NativeWsdlImporterTests
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

        /// <summary>
        /// This test checks that the Native WSDL Importer can handle a monolithic WSDL
        /// (ie) self contained with all types and no xsd:imports etc.
        /// </summary>
        [Test]
        public void Expect_NativeWsdlImporter_Handles_MonolithicWsdl()
        {
            var wsdlImporter = new NativeWsdlImporter();
            var set = MetadataHelper.GetMetadataSetForMonolithicWsdl();
            wsdlImporter.InitializeMetadataSet(set);
            WsdlImportResult result = wsdlImporter.ImportWsdl();
            Assert.IsTrue(result.Endpoints.Count == 1, "Incorrect number of endpoints");
            Assert.IsTrue(result.Bindings.Count == 1, "Incorrect number of bindings");
            Assert.IsTrue(result.Contracts.Count == 1, "Incorrect number of contracts");
        }

        [Test]
        public void Expect_NativeWsdlImporter_Handles_MultipartWsdl()
        {
            var wsdlImporter = new NativeWsdlImporter();
            var set = MetadataHelper.GetMetadataSetForMultipartWsdl();
            wsdlImporter.InitializeMetadataSet(set);
            WsdlImportResult result = wsdlImporter.ImportWsdl();
            Assert.IsTrue(result.Endpoints.Count == 2, "Incorrect number of endpoints");
            Assert.IsTrue(result.Bindings.Count == 2, "Incorrect number of bindings");
            Assert.IsTrue(result.Contracts.Count == 1, "Incorrect number of contracts");
        }
    }
}
