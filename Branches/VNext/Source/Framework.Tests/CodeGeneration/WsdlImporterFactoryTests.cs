using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using Moq;
using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class WsdlImporterFactoryTests
	{
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;
		private readonly XsdDataContractImporter xsdDataContractImporter = new XsdDataContractImporter();
		private readonly XmlSerializerImportOptions xmlSerializerImportOptions = new XmlSerializerImportOptions();
		private readonly WrappedOptions wrappedOptions = new WrappedOptions();
		private readonly FaultImportOptions faultImportOptions = new FaultImportOptions();

		[Test]
		public void Build_MetadataSet_ProvidedToImporter()
		{
			WsdlImporter wsdlImporter = GetWsdlImporter(new CodeGeneratorOptions());

			Assert.That(wsdlImporter.WsdlDocuments, Has.Count.EqualTo(1));
			Assert.That(wsdlImporter.XmlSchemas, Has.Count.EqualTo(3));
		}

		[Test]
		public void Build_WsdlImportExtensions_AutoSerializer()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.Auto};
			WsdlImporter wsdlImporter = GetWsdlImporter(options);

			Type xmlSerializerType = typeof(XmlSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(xmlSerializerType), Is.True);

			Type dataContractType = typeof(DataContractSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(dataContractType), Is.True);
		}

		[Test]
		public void Build_WsdlImportExtensions_XmlSerializer()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.XmlSerializer};
			WsdlImporter wsdlImporter = GetWsdlImporter(options);

			Type xmlSerializerType = typeof(XmlSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(xmlSerializerType), Is.True);

			Type dataContractType = typeof(DataContractSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(dataContractType), Is.False);
		}

		[Test]
		public void Build_WsdlImportExtensions_DataContractSerializer()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.DataContractSerializer};
			WsdlImporter wsdlImporter = GetWsdlImporter(options);

			Type xmlSerializerType = typeof(XmlSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(xmlSerializerType), Is.False);

			Type dataContractType = typeof(DataContractSerializerMessageContractImporter);
			Assert.That(wsdlImporter.WsdlImportExtensions.Contains(dataContractType), Is.True);
		}

		[Test]
		public void State_Serializer_Auto()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.Auto};
			WsdlImporter wsdlImporter = GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			XsdDataContractImporter dataContractImporterState = GetState<XsdDataContractImporter>(wsdlImporter);
			Assert.That(dataContractImporterState, Is.EqualTo(xsdDataContractImporter));

			XmlSerializerImportOptions xmlSerializerState = GetState<XmlSerializerImportOptions>(wsdlImporter);
			Assert.That(xmlSerializerState, Is.EqualTo(xmlSerializerImportOptions));

			WrappedOptions wrappedOptionsState = GetState<WrappedOptions>(wsdlImporter);
			Assert.That(wrappedOptionsState, Is.EqualTo(wrappedOptions));

			FaultImportOptions faultImportOptionsState = GetState<FaultImportOptions>(wsdlImporter);
			Assert.That(faultImportOptionsState, Is.EqualTo(faultImportOptions));

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Exactly(2));
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Once());
		}

		[Test]
		public void State_Serializer_XmlSerializer()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.XmlSerializer};
			WsdlImporter wsdlImporter = GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			XmlSerializerImportOptions xmlSerializerState = GetState<XmlSerializerImportOptions>(wsdlImporter);
			Assert.That(xmlSerializerState, Is.EqualTo(xmlSerializerImportOptions));

			WrappedOptions wrappedOptionsState = GetState<WrappedOptions>(wsdlImporter);
			Assert.That(wrappedOptionsState, Is.EqualTo(wrappedOptions));

			FaultImportOptions faultImportOptionsState = GetState<FaultImportOptions>(wsdlImporter);
			Assert.That(faultImportOptionsState, Is.EqualTo(faultImportOptions));

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Never());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Once());
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Once());
		}

		[Test]
		public void State_Serializer_DataContractSerializer()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {Serializer = SerializerMode.DataContractSerializer};
			WsdlImporter wsdlImporter = GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			XsdDataContractImporter dataContractImporterState = GetState<XsdDataContractImporter>(wsdlImporter);
			Assert.That(dataContractImporterState, Is.EqualTo(xsdDataContractImporter));

			WrappedOptions wrappedOptionsState = GetState<WrappedOptions>(wsdlImporter);
			Assert.That(wrappedOptionsState, Is.EqualTo(wrappedOptions));

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Never());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Once());
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Never());
		}

		[Test]
		public void UseXmlSerializerForFaults_Serializer_Auto()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.Auto,
				UseXmlSerializerForFaults = true
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Exactly(2));
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Once());

			xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();
			
			options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.Auto,
				UseXmlSerializerForFaults = false
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Exactly(2));
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Never());
		}

		[Test]
		public void UseXmlSerializerForFaults_Serializer_XmlSerializer()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.XmlSerializer,
				UseXmlSerializerForFaults = true
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Never());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Once());
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Once());

			xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.XmlSerializer,
				UseXmlSerializerForFaults = false
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Once());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Exactly(2));
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Never());
		}

		[Test]
		public void UseXmlSerializerForFaults_Serializer_DataContractSerializer()
		{
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.DataContractSerializer,
				UseXmlSerializerForFaults = true
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Never());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Once());
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Never());

			xmlSerializerImportOptionsBuilder = CreateXmlSerializerImportOptionsBuilder();
			xsdDataContractImporterBuilder = CreateXsdDataContractImporterBuilder();
			wrappedOptionsBuilder = CreateWrappedOptionsBuilder();
			faultImportOptionsBuilder = CreateFaultImportOptionsBuilder();

			options = new CodeGeneratorOptions
			{
				Serializer = SerializerMode.DataContractSerializer,
				UseXmlSerializerForFaults = false
			};
			GetWsdlImporter(options, xmlSerializerImportOptionsBuilder, xsdDataContractImporterBuilder, wrappedOptionsBuilder, faultImportOptionsBuilder);

			VerifyXmlSerializerImportOptionsBuilder(xmlSerializerImportOptionsBuilder, Times.Never());
			VerifyXsdDataContractImporterBuilder(xsdDataContractImporterBuilder, Times.Once());
			VerifyWrappedOptionsBuilder(wrappedOptionsBuilder, Times.Once());
			VerifyFaultImportOptionsBuilder(faultImportOptionsBuilder, Times.Never());
		}

		private WsdlImporter GetWsdlImporter(CodeGeneratorOptions options)
		{
			return GetWsdlImporter(options,
				CreateXmlSerializerImportOptionsBuilder(),
				CreateXsdDataContractImporterBuilder(),
				CreateWrappedOptionsBuilder(),
				CreateFaultImportOptionsBuilder());
		}

		private WsdlImporter GetWsdlImporter(
			CodeGeneratorOptions options,
			Mock<IXmlSerializerImportOptionsBuilder> xmlSerializerImportOptionsBuilder,
			Mock<IXsdDataContractImporterBuilder> xsdDataContractImporterBuilder,
			Mock<IWrappedOptionsBuilder> wrappedOptionsBuilder,
			Mock<IFaultImportOptionsBuilder> faultImportOptionsBuilder)
		{
			WsdlImporterFactory factory = new WsdlImporterFactory(
				xmlSerializerImportOptionsBuilder.Object,
				xsdDataContractImporterBuilder.Object,
				wrappedOptionsBuilder.Object,
				faultImportOptionsBuilder.Object);

			ICodeGeneratorContext codeGeneratorContext = CreateCodeGeneratorContext(options);

			return factory.GetWsdlImporter(codeGeneratorContext);
		}

		private ICodeGeneratorContext CreateCodeGeneratorContext(CodeGeneratorOptions codeGeneratorOptions)
		{
			return new CodeGeneratorContext(metadataSet, codeGeneratorOptions);
		}

		private Mock<IXsdDataContractImporterBuilder> CreateXsdDataContractImporterBuilder()
		{
			Mock<IXsdDataContractImporterBuilder> builder = new Mock<IXsdDataContractImporterBuilder>();
			builder.Setup(mock => mock.Build(It.IsAny<ICodeGeneratorContext>())).Returns(xsdDataContractImporter);
			return builder;
		}

		private Mock<IXmlSerializerImportOptionsBuilder> CreateXmlSerializerImportOptionsBuilder()
		{
			Mock<IXmlSerializerImportOptionsBuilder> builder = new Mock<IXmlSerializerImportOptionsBuilder>();
			builder.Setup(mock => mock.Build(It.IsAny<ICodeGeneratorContext>())).Returns(xmlSerializerImportOptions);
			return builder;
		}

		private Mock<IWrappedOptionsBuilder> CreateWrappedOptionsBuilder()
		{
			Mock<IWrappedOptionsBuilder> builder = new Mock<IWrappedOptionsBuilder>();
			builder.Setup(mock => mock.Build(It.IsAny<CodeGeneratorOptions>())).Returns(wrappedOptions);
			return builder;
		}

		private Mock<IFaultImportOptionsBuilder> CreateFaultImportOptionsBuilder()
		{
			Mock<IFaultImportOptionsBuilder> builder = new Mock<IFaultImportOptionsBuilder>();
			builder.Setup(mock => mock.Build()).Returns(faultImportOptions);
			return builder;
		}

		private static void VerifyXsdDataContractImporterBuilder(Mock<IXsdDataContractImporterBuilder> builder, Times times)
		{
			builder.Verify(mock => mock.Build(It.IsAny<ICodeGeneratorContext>()), times);
		}

		private static void VerifyXmlSerializerImportOptionsBuilder(Mock<IXmlSerializerImportOptionsBuilder> builder, Times times)
		{
			builder.Verify(mock => mock.Build(It.IsAny<ICodeGeneratorContext>()), times);
		}

		private static void VerifyWrappedOptionsBuilder(Mock<IWrappedOptionsBuilder> builder, Times times)
		{
			builder.Verify(mock => mock.Build(It.IsAny<CodeGeneratorOptions>()), times);
		}

		private static void VerifyFaultImportOptionsBuilder(Mock<IFaultImportOptionsBuilder> builder, Times times)
		{
			builder.Verify(mock => mock.Build(), times);
		}

		private static T GetState<T>(MetadataImporter wsdlImporter) where T : class
		{
			object state;
			wsdlImporter.State.TryGetValue(typeof(T), out state);
			return state as T;
		}
	}
}