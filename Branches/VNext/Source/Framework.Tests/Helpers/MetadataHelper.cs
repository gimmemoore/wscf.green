using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Schema;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Thinktecture.Wscf.Framework.Tests.Helpers
{
    using Thinktecture.Wscf.Framework.Contract;

    internal static class MetadataHelper
    {
        internal static MetadataSet GetMetadataSetForMultipartWsdl()
        {
            string xmlSchemaFile = TestFiles.GetFilePath(TestFiles.RestaurantDataXsdFileName);
            XmlSchema xmlSchema;
            using (XmlTextReader xmlTextReader = new XmlTextReader(xmlSchemaFile))
            {
                xmlSchema = XmlSchema.Read(xmlTextReader, null);
            }

            string xmlHeaderSchemaFile = TestFiles.GetFilePath(TestFiles.RestaurantHeaderDataXsdFileName);
            XmlSchema headerSchema;
            using (XmlTextReader xmlTextReader = new XmlTextReader(xmlHeaderSchemaFile))
            {
                headerSchema = XmlSchema.Read(xmlTextReader, null);
            }

            string xmlMessagesSchemaFile = TestFiles.GetFilePath(TestFiles.RestaurantMessagesXsdFileName);
            XmlSchema messageSchema;
            using (XmlTextReader xmlTextReader = new XmlTextReader(xmlMessagesSchemaFile))
            {
                messageSchema = XmlSchema.Read(xmlTextReader, null);
            }


            string serviceDescriptionFile = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName);
            ServiceDescription serviceDescription;
            using (XmlTextReader xmlTextReader = new XmlTextReader(serviceDescriptionFile))
            {
                serviceDescription = ServiceDescription.Read(xmlTextReader);
            }

            List<MetadataSection> sections = new List<MetadataSection>
			{
				MetadataSection.CreateFromSchema(xmlSchema),
                MetadataSection.CreateFromSchema(headerSchema),
                MetadataSection.CreateFromSchema(messageSchema),
				MetadataSection.CreateFromServiceDescription(serviceDescription)
			};

            return new MetadataSet(sections);
        }

        internal static MetadataSet GetMetadataSetForMonolithicWsdl()
        {
            string xmlSchemaFile = TestFiles.GetFilePath(TestFiles.RestaurantDataXsdFileName);
            XmlSchema xmlSchema;
            using (XmlTextReader xmlTextReader = new XmlTextReader(xmlSchemaFile))
            {
                xmlSchema = XmlSchema.Read(xmlTextReader, null);
            }

            string serviceDescriptionFile = TestFiles.GetFilePath(TestFiles.RestaurantServiceWsdlFileName2);
            ServiceDescription serviceDescription;
            using (XmlTextReader xmlTextReader = new XmlTextReader(serviceDescriptionFile))
            {
                serviceDescription = ServiceDescription.Read(xmlTextReader);
            }

            List<MetadataSection> sections = new List<MetadataSection>
			{
				MetadataSection.CreateFromSchema(xmlSchema),
				MetadataSection.CreateFromServiceDescription(serviceDescription)
			};

            return new MetadataSet(sections);
        }

        
        internal static WsdlImportResult GetImportResult(MetadataSet metadataSet)
        {
            WsdlImporter fxWsdlImporter = new WsdlImporter(metadataSet);
            WsdlImportResult result = new WsdlImportResult();
            result.Endpoints = fxWsdlImporter.ImportAllEndpoints();
            result.Bindings = fxWsdlImporter.ImportAllBindings();
            result.Contracts = fxWsdlImporter.ImportAllContracts();
            return result;
        }

    }
}
