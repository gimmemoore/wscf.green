using System.Collections.Generic;
using System.Data.Design;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml.Serialization;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class XmlSerializerImportOptionsBuilderTests
	{
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;

		[Test]
		public void Build_CodeGenerationOptions_DefaultsToNone()
		{
			XmlSerializerImportOptions importOptions = PerformBuild(new CodeGeneratorOptions());

			Assert.That(importOptions.WebReferenceOptions.CodeGenerationOptions, Is.EqualTo(CodeGenerationOptions.None));
		}

		[Test]
		public void Build_CodeGeneratorContext_CodeDomProvider()
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());
			IXmlSerializerImportOptionsBuilder builder = new XmlSerializerImportOptionsBuilder();
			XmlSerializerImportOptions importOptions = builder.Build(codeGeneratorContext);

			Assert.That(importOptions.CodeProvider, Is.EqualTo(codeGeneratorContext.CodeDomProvider));
		}

		[Test]
		public void Build_CodeGeneratorContext_CodeCompileUnit()
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());
			IXmlSerializerImportOptionsBuilder builder = new XmlSerializerImportOptionsBuilder();
			XmlSerializerImportOptions importOptions = builder.Build(codeGeneratorContext);

			Assert.That(importOptions.CodeCompileUnit, Is.EqualTo(codeGeneratorContext.CodeCompileUnit));
		}

		[Test]
		public void Build_CodeGeneratorOptions_EnableDataBinding()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {EnableDataBinding = true};
			XmlSerializerImportOptions importOptions = PerformBuild(options);

			Assert.That(importOptions.WebReferenceOptions.CodeGenerationOptions, Is.EqualTo(CodeGenerationOptions.EnableDataBinding));
		}

		[Test]
		public void Build_CodeGeneratorOptions_GenerateProperties()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {GenerateProperties = true};
			XmlSerializerImportOptions importOptions = PerformBuild(options);

			Assert.That(importOptions.WebReferenceOptions.CodeGenerationOptions, Is.EqualTo(CodeGenerationOptions.GenerateProperties));
		}

		[Test]
		public void Build_CodeGeneratorOptions_NamespaceMappings()
		{
			const string expectedNamespace = "TestNamespace";
			Dictionary<string, string> mappings = new Dictionary<string, string> {{"*", expectedNamespace}};
			CodeGeneratorOptions options = new CodeGeneratorOptions {NamespaceMappings = mappings};
			XmlSerializerImportOptions importOptions = PerformBuild(options);

			Assert.That(importOptions.ClrNamespace, Is.EqualTo(expectedNamespace));
		}

		[Test]
		public void Build_CodeGeneratorOptions_TargetFrameworkVersion()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {TargetFrameworkVersion = TargetFrameworkVersion.Version35};
			XmlSerializerImportOptions importOptions = PerformBuild(options);

			string extensionAssemblyQualifiedName = typeof(TypedDataSetSchemaImporterExtensionFx35).AssemblyQualifiedName;
			Assert.That(importOptions.WebReferenceOptions.SchemaImporterExtensions, Has.Member(extensionAssemblyQualifiedName));

			options = new CodeGeneratorOptions {TargetFrameworkVersion = TargetFrameworkVersion.Version30};
			importOptions = PerformBuild(options);

			extensionAssemblyQualifiedName = typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName;
			Assert.That(importOptions.WebReferenceOptions.SchemaImporterExtensions, Has.Member(extensionAssemblyQualifiedName));
		}

		[Test]
		public void Build_CodeGeneratorOptions_OrderMembers()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {OrderMembers = true};
			XmlSerializerImportOptions importOptions = PerformBuild(options);

			Assert.That(importOptions.WebReferenceOptions.CodeGenerationOptions, Is.EqualTo(CodeGenerationOptions.GenerateOrder));
		}

		private XmlSerializerImportOptions PerformBuild(CodeGeneratorOptions codeGeneratorOptions)
		{
			ICodeGeneratorContext codeGeneratorContext = new CodeGeneratorContext(metadataSet, codeGeneratorOptions);
			IXmlSerializerImportOptionsBuilder builder = new XmlSerializerImportOptionsBuilder();
			return builder.Build(codeGeneratorContext);
		}
	}
}