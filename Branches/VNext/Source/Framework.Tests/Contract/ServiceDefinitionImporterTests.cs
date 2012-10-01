using Moq;
using NUnit.Framework;

namespace Thinktecture.Wscf.Framework.Contract
{
    using Thinktecture.Wscf.Framework.Metadata;
    using Thinktecture.Wscf.Framework.Tests.Helpers;

    [TestFixture]
    public class ServiceDefinitionImporterTests
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
        /// Tests that the SUT will call the discovery agent once with the supplied URL.
        /// </summary>
        [Test]
        public void Expect_Call_To_Discovery_Agent_To_Process_Url()
        {
            string path = "dummy";

            var mockDiscoveryAgent = new Mock<IMetadataDiscovery>();
            var mockWsdlImporter = new Mock<IWsdlImporter>();
            var importer = new ServiceDefinitionImporter(mockDiscoveryAgent.Object, mockWsdlImporter.Object);
            var set = MetadataHelper.GetMetadataSetForMultipartWsdl();
            var result=  MetadataHelper.GetImportResult(set);
            mockDiscoveryAgent.Setup(mock => mock.Process(path)).Returns(set);
            mockWsdlImporter.Setup(mock => mock.ImportWsdl()).Returns(result);

            importer.Import(path);
            mockDiscoveryAgent.Verify(mock => mock.Process(path), Times.Once());
        }

        /// <summary>
        /// Tests that the SUT will call the importer only once 
        /// </summary>
        [Test]
        public void Expect_One_Call_To_Importer()
        {
            string path = "dummy";

            var mockDiscoveryAgent = new Mock<IMetadataDiscovery>();
            var mockWsdlImporter = new Mock<IWsdlImporter>();
            var importer = new ServiceDefinitionImporter(mockDiscoveryAgent.Object, mockWsdlImporter.Object);
            var set = MetadataHelper.GetMetadataSetForMultipartWsdl();
            var result = MetadataHelper.GetImportResult(set);
            mockDiscoveryAgent.Setup(mock => mock.Process(path)).Returns(set);
            mockWsdlImporter.Setup(mock => mock.ImportWsdl()).Returns(result);

            importer.Import(path);
            mockWsdlImporter.Verify(mock => mock.ImportWsdl(), Times.Once());
        }

        /// <summary>
        /// This tests checks if the SUT can build a valid service definition from the 
        /// imported data. At this point it doesnt matter if a single or multipart wsdl
        /// is involved because the importer handles all that. 
        /// </summary>
        [Test]
        public void Expect_SUT_Builds_Valid_ServiceDefinition_FromImport()
        {

        }
    }
}
