using System.ServiceModel.Description;

using NUnit.Framework;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class ServiceContractGenerationOptionsBuilderTests
	{
		[Test]
		public void Build_CodeGenerationOptions_DefaultsToNone()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();
			ServiceContractGenerationOptions contractOptions = builder.Build(new CodeGeneratorOptions());

			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.None));
		}

		[Test]
		public void Build_CodeGeneratorOptions_AsyncMethods()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {AsyncMethods = true};
			ServiceContractGenerationOptions contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.AsynchronousMethods));

			options = new CodeGeneratorOptions {AsyncMethods = false};
			contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.None));
		}

		[Test]
		public void Build_CodeGeneratorOptions_AsyncMethodsWithTargetClientVersion()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions
			{
				AsyncMethods = true, TargetFrameworkVersion = TargetFrameworkVersion.Version35
			};
			ServiceContractGenerationOptions contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.AsynchronousMethods | ServiceContractGenerationOptions.EventBasedAsynchronousMethods));

			options = new CodeGeneratorOptions
			{
				AsyncMethods = true,
				TargetFrameworkVersion = TargetFrameworkVersion.Version30
			};
			contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.AsynchronousMethods));
		}

		[Test]
		public void Build_CodeGeneratorOptions_CodeGeneratorMode()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {CodeGeneratorMode = CodeGeneratorMode.Client};
			ServiceContractGenerationOptions contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.ClientClass | ServiceContractGenerationOptions.ChannelInterface));

			options = new CodeGeneratorOptions {CodeGeneratorMode = CodeGeneratorMode.Service};
			contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.None));
		}

		[Test]
		public void Build_CodeGeneratorOptions_InternalTypes()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {InternalTypes = true};
			ServiceContractGenerationOptions contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.InternalTypes));

			options = new CodeGeneratorOptions {InternalTypes = false};
			contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.None));
		}

		[Test]
		public void Build_CodeGeneratorOptions_TypedMessages()
		{
			IServiceContractGenerationOptionsBuilder builder = new ServiceContractGenerationOptionsBuilder();

			CodeGeneratorOptions options = new CodeGeneratorOptions {TypedMessages = true};
			ServiceContractGenerationOptions contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.TypedMessages));

			options = new CodeGeneratorOptions {TypedMessages = false};
			contractOptions = builder.Build(options);
			Assert.That(contractOptions, Is.EqualTo(ServiceContractGenerationOptions.None));
		}
	}
}