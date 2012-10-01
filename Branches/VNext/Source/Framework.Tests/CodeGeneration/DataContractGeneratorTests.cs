using System.CodeDom;
using System.Runtime.Serialization;
using System.ServiceModel.Description;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class DataContractGeneratorTests
	{
		private CodeGeneratorOptions codeGeneratorOptions;
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;
		
		[SetUp]
		public void SetUp()
		{
			codeGeneratorOptions = BuildDefaultCodeGeneratorOptions();
		}
		
		[Test]
		public void ImportXmlTypesOptionForcesAllTypesToBeImported()
		{
			codeGeneratorOptions.ImportXmlTypes = false;
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, codeGeneratorOptions);

			XsdDataContractImporterBuilder xsdDataContractImporterBuilder = new XsdDataContractImporterBuilder();
			IDataContractGenerator dataContractGenerator = new DataContractGenerator(xsdDataContractImporterBuilder);

			Assert.Throws<InvalidDataContractException>(() => dataContractGenerator.GenerateCode(codeGeneratorContext));

			codeGeneratorOptions.ImportXmlTypes = true;
			CodeCompileUnit codeCompileUnit = dataContractGenerator.GenerateCode(codeGeneratorContext);

			Assert.That(codeCompileUnit.Namespaces, Has.Count.EqualTo(3));
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