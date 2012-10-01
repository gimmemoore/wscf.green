using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class XsdDataContractImporterBuilderTests
	{
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;

		[Test]
		public void Build_CodeGeneratorContext_CodeDomProvider()
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());
			IXsdDataContractImporterBuilder builder = new XsdDataContractImporterBuilder();
			XsdDataContractImporter importer = builder.Build(codeGeneratorContext);

			Assert.That(importer.Options.CodeProvider, Is.EqualTo(codeGeneratorContext.CodeDomProvider));
		}

		[Test]
		public void Build_CodeGeneratorContext_CodeCompileUnit()
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());
			IXsdDataContractImporterBuilder builder = new XsdDataContractImporterBuilder();
			XsdDataContractImporter importer = builder.Build(codeGeneratorContext);

			Assert.That(importer.CodeCompileUnit, Is.EqualTo(codeGeneratorContext.CodeCompileUnit));
		}

		[Test]
		public void Build_CodeGeneratorOptions_EnableDataBinding()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {EnableDataBinding = true};
			XsdDataContractImporter importer = PerformBuild(options);

			Assert.That(importer.Options.EnableDataBinding, Is.True);

			options = new CodeGeneratorOptions {EnableDataBinding = false};
			importer = PerformBuild(options);

			Assert.That(importer.Options.EnableDataBinding, Is.False);
		}

		[Test]
		public void Build_CodeGeneratorOptions_ImportXmlTypes()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {ImportXmlTypes = true};
			XsdDataContractImporter importer = PerformBuild(options);

			Assert.That(importer.Options.ImportXmlType, Is.True);

			options = new CodeGeneratorOptions {ImportXmlTypes = false};
			importer = PerformBuild(options);

			Assert.That(importer.Options.ImportXmlType, Is.False);
		}

		[Test]
		public void Build_CodeGeneratorOptions_InternalTypes()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {InternalTypes = true};
			XsdDataContractImporter importer = PerformBuild(options);

			Assert.That(importer.Options.GenerateInternal, Is.True);

			options = new CodeGeneratorOptions {InternalTypes = false};
			importer = PerformBuild(options);

			Assert.That(importer.Options.GenerateInternal, Is.False);
		}

		[Test]
		public void Build_CodeGeneratorOptions_NamespaceMappings()
		{
			const string expectedMappingKey1 = "1";
			const string expectedMappingKey2 = "2";
			const string expectedNamespace1 = "Namespace1";
			const string expectedNamespace2 = "Namespace2";

			Dictionary<string, string> mappings = new Dictionary<string, string>
			{
				{expectedMappingKey1, expectedNamespace1},
				{expectedMappingKey2, expectedNamespace2}
			};
			CodeGeneratorOptions options = new CodeGeneratorOptions {NamespaceMappings = mappings};
			XsdDataContractImporter importer = PerformBuild(options);
			IDictionary<string, string> namespaces = importer.Options.Namespaces;

			Assert.That(namespaces, Has.Count.EqualTo(2));
			Assert.That(namespaces[expectedMappingKey1], Is.EqualTo(expectedNamespace1));
			Assert.That(namespaces[expectedMappingKey2], Is.EqualTo(expectedNamespace2));
		}

		[Test]
		public void Build_CodeGeneratorOptions_SerializableAttribute()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {SerializableAttribute = true};
			XsdDataContractImporter importer = PerformBuild(options);

			Assert.That(importer.Options.GenerateSerializable, Is.True);

			options = new CodeGeneratorOptions {SerializableAttribute = false};
			importer = PerformBuild(options);

			Assert.That(importer.Options.GenerateSerializable, Is.False);
		}

		private XsdDataContractImporter PerformBuild(CodeGeneratorOptions codeGeneratorOptions)
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, codeGeneratorOptions);
			XsdDataContractImporterBuilder builder = new XsdDataContractImporterBuilder();
			return builder.Build(codeGeneratorContext);
		}
	}
}