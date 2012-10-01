using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A builder for creating <see cref="ServiceContractGenerationOptions"/> instances.
	/// </summary>
	public class ServiceContractGenerationOptionsBuilder : IServiceContractGenerationOptionsBuilder
	{
		/// <summary>
		/// Builds a new instance.
		/// </summary>
		/// <param name="codeGeneratorOptions">The code generator options.</param>
		/// <returns>
		/// A new <see cref="ServiceContractGenerationOptions"/> instance.
		/// </returns>
		public ServiceContractGenerationOptions Build(CodeGeneratorOptions codeGeneratorOptions)
		{
			ServiceContractGenerationOptions serviceContractGenerationOptions = new ServiceContractGenerationOptions();

			if (codeGeneratorOptions.AsyncMethods)
			{
				serviceContractGenerationOptions |= ServiceContractGenerationOptions.AsynchronousMethods;
				if (codeGeneratorOptions.TargetFrameworkVersion == TargetFrameworkVersion.Version35)
				{
					serviceContractGenerationOptions |= ServiceContractGenerationOptions.EventBasedAsynchronousMethods;
				}
			}

			if (codeGeneratorOptions.CodeGeneratorMode == CodeGeneratorMode.Client)
			{
				serviceContractGenerationOptions |= (ServiceContractGenerationOptions.ClientClass | ServiceContractGenerationOptions.ChannelInterface);
			}

			if (codeGeneratorOptions.InternalTypes)
			{
				serviceContractGenerationOptions |= ServiceContractGenerationOptions.InternalTypes;
			}

			if (codeGeneratorOptions.TypedMessages)
			{
				serviceContractGenerationOptions |= ServiceContractGenerationOptions.TypedMessages;
			}

			return serviceContractGenerationOptions;
		}
	}
}