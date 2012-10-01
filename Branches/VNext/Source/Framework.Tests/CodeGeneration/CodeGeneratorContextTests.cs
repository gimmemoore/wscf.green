using System.ServiceModel.Description;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

using NUnit.Framework;
using Thinktecture.Wscf.Framework.Tests.Helpers;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class CodeGeneratorContextTests
	{
		private readonly MetadataSet metadataSet = TestMetadata.MetadataSet;

		[Test]
		public void CodeCompileUnitPropertyIsInitialized()
		{
			ICodeGeneratorContext context = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());

			Assert.That(context.CodeCompileUnit, Is.Not.Null);
		}

		[Test]
		public void MetadataPropertyIsAssignedConstructorArgument()
		{
			ICodeGeneratorContext context = new CodeGeneratorContext(metadataSet, new CodeGeneratorOptions());

			Assert.That(context.MetadataSet, Is.EqualTo(metadataSet));
		}

		[Test]
		public void CodeGenerationOptionsPropertyIsAssignedConstructorArgument()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			ICodeGeneratorContext context = new CodeGeneratorContext(metadataSet, options);

			Assert.That(context.CodeGeneratorOptions, Is.EqualTo(options));
		}

		[Test]
		public void CodeDomProviderMatchesTheCodeLanguageOption()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {CodeLanguage = CodeLanguage.CSharp};
			ICodeGeneratorContext context = new CodeGeneratorContext(metadataSet, options);

			Assert.That(context.CodeDomProvider, Is.InstanceOf<CSharpCodeProvider>());

			options = new CodeGeneratorOptions {CodeLanguage = CodeLanguage.VisualBasic};
			context = new CodeGeneratorContext(metadataSet, options);

			Assert.That(context.CodeDomProvider, Is.InstanceOf<VBCodeProvider>());
		}
	}
}