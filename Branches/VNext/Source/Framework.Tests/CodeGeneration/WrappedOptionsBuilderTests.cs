using System.ServiceModel.Channels;

using NUnit.Framework;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class WrappedOptionsBuilderTests
	{
		[Test]
		public void Build_CodeGeneratorOptions_Wrapped()
		{
			CodeGeneratorOptions options = new CodeGeneratorOptions {Wrapped = true};
			WrappedOptions wrappedOptions = PerformBuild(options);

			Assert.That(wrappedOptions.WrappedFlag, Is.True);

			options = new CodeGeneratorOptions {Wrapped = false};
			wrappedOptions = PerformBuild(options);

			Assert.That(wrappedOptions.WrappedFlag, Is.False);
		}

		private static WrappedOptions PerformBuild(CodeGeneratorOptions codeGeneratorOptions)
		{
			IWrappedOptionsBuilder builder = new WrappedOptionsBuilder();
			return builder.Build(codeGeneratorOptions);
		}
	}
}