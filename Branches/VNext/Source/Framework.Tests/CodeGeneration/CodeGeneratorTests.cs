using System.CodeDom;
using System.Configuration;
using System.ServiceModel.Description;
using System.Linq;

using Moq;
using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class CodeGeneratorTests
	{
		private CodeGeneratorOptions codeGeneratorOptions;
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;

		[SetUp]
		public void SetUp()
		{
			codeGeneratorOptions = BuildDefaultCodeGeneratorOptions();
		}

		[Test]
		public void CodeGeneratorContextProvidedToFactoryMethods()
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, codeGeneratorOptions);

			Mock<IWsdlImporterFactory> wsdlImporterFactory = new Mock<IWsdlImporterFactory>();
			wsdlImporterFactory.Setup(mock => mock.GetWsdlImporter(codeGeneratorContext))
				.Returns(new WsdlImporter(metadataSet)).AtMostOnce();

			Mock<IServiceContractGeneratorFactory> contractGeneratorFactory = new Mock<IServiceContractGeneratorFactory>();
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			contractGeneratorFactory.Setup(mock => mock.GetServiceContractGenerator(codeGeneratorContext))
				.Returns(new ServiceContractGenerator(codeGeneratorContext.CodeCompileUnit, configuration)).AtMostOnce();

			ICodeGenerator codeGenerator = new CodeGenerator(wsdlImporterFactory.Object, contractGeneratorFactory.Object);
			codeGenerator.GenerateCode(codeGeneratorContext);

			wsdlImporterFactory.Verify();
			contractGeneratorFactory.Verify();
		}

		[Test]
		public void GeneratingTheServiceGeneratesTheServiceInterface()
		{
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int serviceInterfaceCount = codeCompileUnit.Namespaces.OfType<CodeNamespace>()
				.SelectMany(codeNamespace => codeNamespace.Types.OfType<CodeTypeDeclaration>())
				.Where(type => type.Name == "IRestaurantService")
				.SelectMany(type => type.CustomAttributes.OfType<CodeAttributeDeclaration>())
				.Count(attribute => attribute.Name == "System.ServiceModel.ServiceContractAttribute");
			Assert.That(serviceInterfaceCount, Is.EqualTo(1));
		}

		[Test]
		public void CodeCompileUnitNamespacesTakenFromNamespaceMappings()
		{
			const string clrNamespace = "UnitTesting";

			codeGeneratorOptions.NamespaceMappings.Add("*", clrNamespace);
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			Assert.That(codeCompileUnit.Namespaces, Has.Count.EqualTo(1));
			Assert.That(codeCompileUnit.Namespaces[0].Name, Is.EqualTo(clrNamespace));
		}

		[Test]
		public void GeneratingTheServiceDoesNotGenerateChannelAndClient()
		{
			codeGeneratorOptions.CodeGeneratorMode = CodeGeneratorMode.Service;
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int channelCount = codeCompileUnit.Namespaces
				.OfType<CodeNamespace>()
				.SelectMany(cn => cn.Types.OfType<CodeTypeDeclaration>())
				.Count(type => type.Name == "IRestaurantServiceChannel");
			Assert.That(channelCount, Is.EqualTo(0));

			int clientCount = codeCompileUnit.Namespaces
				.OfType<CodeNamespace>()
				.SelectMany(cn => cn.Types.OfType<CodeTypeDeclaration>())
				.Count(type => type.Name == "RestaurantServiceClient");
			Assert.That(clientCount, Is.EqualTo(0));
		}

		[Test]
		public void GeneratingTheClientAlsoGeneratesChannel()
		{
			codeGeneratorOptions.CodeGeneratorMode = CodeGeneratorMode.Client;
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int channelCount = codeCompileUnit.Namespaces
				.OfType<CodeNamespace>()
				.SelectMany(cn => cn.Types.OfType<CodeTypeDeclaration>())
				.Count(type => type.Name == "IRestaurantServiceChannel");
			Assert.That(channelCount, Is.EqualTo(1));

			int clientCount = codeCompileUnit.Namespaces
				.OfType<CodeNamespace>()
				.SelectMany(cn => cn.Types.OfType<CodeTypeDeclaration>())
				.Count(type => type.Name == "RestaurantServiceClient");
			Assert.That(clientCount, Is.EqualTo(1));
		}

		[Test]
		public void XmlSerializerOptionResultsInSerializableTypes()
		{
			codeGeneratorOptions.Serializer = SerializerMode.XmlSerializer;
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int typeCount = codeCompileUnit.Namespaces.OfType<CodeNamespace>()
				.SelectMany(codeNamespace => codeNamespace.Types.OfType<CodeTypeDeclaration>())
				.Where(type => type.Name == "getRestaurants")
				.SelectMany(type => type.CustomAttributes.OfType<CodeAttributeDeclaration>())
				.Count(attribute => attribute.Name == "System.SerializableAttribute");

			Assert.That(typeCount, Is.EqualTo(1));
		}

		[Test]
		public void NonSupportedTypeNotGeneratedWithDataContractSerializer()
		{
			codeGeneratorOptions.Serializer = SerializerMode.DataContractSerializer;
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int typeCount = codeCompileUnit.Namespaces.OfType<CodeNamespace>()
				.SelectMany(codeNamespace => codeNamespace.Types.OfType<CodeTypeDeclaration>())
				.Count(type => type.Name == "getRestaurants");
			Assert.That(typeCount, Is.EqualTo(0));
		}

		[Test]
		public void AutoSerializerOptionFallsBackToXmlSerializerForNonSupportedType()
		{
			codeGeneratorOptions.Serializer = SerializerMode.Auto;
			CodeCompileUnit codeCompileUnit = GenerateCode(codeGeneratorOptions);

			int typeCount = codeCompileUnit.Namespaces.OfType<CodeNamespace>()
				.SelectMany(codeNamespace => codeNamespace.Types.OfType<CodeTypeDeclaration>())
				.Where(type => type.Name == "getRestaurants")
				.SelectMany(type => type.CustomAttributes.OfType<CodeAttributeDeclaration>())
				.Count(attribute => attribute.Name == "System.SerializableAttribute");

			Assert.That(typeCount, Is.EqualTo(1));
		}

		private CodeCompileUnit GenerateCode(CodeGeneratorOptions options)
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, options);

			WsdlImporterFactory wsdlImporterFactory = new WsdlImporterFactory(
				new XmlSerializerImportOptionsBuilder(),
				new XsdDataContractImporterBuilder(),
				new WrappedOptionsBuilder(),
				new FaultImportOptionsBuilder());

			ServiceContractGeneratorFactory contractGeneratorFactory = new ServiceContractGeneratorFactory(
				new ServiceContractGenerationOptionsBuilder());

			ICodeGenerator codeGenerator = new CodeGenerator(wsdlImporterFactory, contractGeneratorFactory);

			return codeGenerator.GenerateCode(codeGeneratorContext);
		}

		private static CodeGeneratorOptions BuildDefaultCodeGeneratorOptions()
		{
			return new CodeGeneratorOptions
			{
				AsyncMethods = false,
				CodeGeneratorMode = CodeGeneratorMode.Service,
				CodeLanguage = CodeLanguage.CSharp,
				EnableDataBinding = false,
				ImportXmlTypes = true,
				InternalTypes = false,
				Serializer = SerializerMode.Auto,
				TargetFrameworkVersion = TargetFrameworkVersion.Version35,
				TypedMessages = true,
				UseXmlSerializerForFaults = true,
				Wrapped = true
			};
		}
	}
}