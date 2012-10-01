using System;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// A factory for creating <see cref="WsdlImporter"/> instances.
	/// </summary>
	public class WsdlImporterFactory : IWsdlImporterFactory
	{
		private readonly IXmlSerializerImportOptionsBuilder xmlSerializerImportOptionsBuilder;
		private readonly IXsdDataContractImporterBuilder xsdDataContractImporterBuilder;
		private readonly IWrappedOptionsBuilder wrappedOptionsBuilder;
		private readonly IFaultImportOptionsBuilder faultImportOptionsBuilder;

		/// <summary>
		/// Initializes a new instance of the <see cref="WsdlImporterFactory"/> class.
		/// </summary>
		/// <param name="xmlSerializerImportOptionsBuilder">The XML serializer import options builder.</param>
		/// <param name="xsdDataContractImporterBuilder">The XSD data contract importer builder.</param>
		/// <param name="wrappedOptionsBuilder">The wrapped options builder.</param>
		/// <param name="faultImportOptionsBuilder">The fault import options builder.</param>
		public WsdlImporterFactory(IXmlSerializerImportOptionsBuilder xmlSerializerImportOptionsBuilder, IXsdDataContractImporterBuilder xsdDataContractImporterBuilder,
			IWrappedOptionsBuilder wrappedOptionsBuilder, IFaultImportOptionsBuilder faultImportOptionsBuilder)
		{
			this.xmlSerializerImportOptionsBuilder = xmlSerializerImportOptionsBuilder;
			this.xsdDataContractImporterBuilder = xsdDataContractImporterBuilder;
			this.wrappedOptionsBuilder = wrappedOptionsBuilder;
			this.faultImportOptionsBuilder = faultImportOptionsBuilder;
		}

		/// <summary>
		/// Gets a new <see cref="WsdlImporter"/> instance.
		/// </summary>
		/// <param name="codeGeneratorContext">The code generator context.</param>
		/// <returns>
		/// A new <see cref="WsdlImporter"/> instance.
		/// </returns>
		public WsdlImporter GetWsdlImporter(ICodeGeneratorContext codeGeneratorContext)
		{
			WsdlImporter wsdlImporter = new WsdlImporter(codeGeneratorContext.MetadataSet);

			RemoveUnneededSerializers(wsdlImporter, codeGeneratorContext);
			ConfigureSerializers(wsdlImporter, codeGeneratorContext);

			return wsdlImporter;
		}

		private static void RemoveUnneededSerializers(WsdlImporter wsdlImporter, ICodeGeneratorContext codeGeneratorContext)
		{
			CodeGeneratorOptions codeGeneratorOptions = codeGeneratorContext.CodeGeneratorOptions;

			switch (codeGeneratorOptions.Serializer)
			{
				case SerializerMode.Auto:
					break;

				case SerializerMode.DataContractSerializer:
					RemoveExtension(wsdlImporter, typeof(XmlSerializerMessageContractImporter));
					return;

				case SerializerMode.XmlSerializer:
					RemoveExtension(wsdlImporter, typeof(DataContractSerializerMessageContractImporter));
					return;
			}
		}

		private static void RemoveExtension(WsdlImporter wsdlImporter, Type extensionType)
		{
			wsdlImporter.WsdlImportExtensions.Remove(extensionType);
		}

		private void ConfigureSerializers(MetadataImporter wsdlImporter, ICodeGeneratorContext codeGeneratorContext)
		{
			CodeGeneratorOptions codeGeneratorOptions = codeGeneratorContext.CodeGeneratorOptions;

			switch (codeGeneratorOptions.Serializer)
			{
				case SerializerMode.Auto:
					AddStateForDataContractSerializerImport(wsdlImporter, codeGeneratorContext);
					AddStateForXmlSerializerImport(wsdlImporter, codeGeneratorContext);
					if (!codeGeneratorOptions.UseXmlSerializerForFaults)
					{
						break;
					}
					AddStateForFaultSerializerImport(wsdlImporter);
					return;

				case SerializerMode.DataContractSerializer:
					AddStateForDataContractSerializerImport(wsdlImporter, codeGeneratorContext);
					return;

				case SerializerMode.XmlSerializer:
					AddStateForXmlSerializerImport(wsdlImporter, codeGeneratorContext);
					if (!codeGeneratorOptions.UseXmlSerializerForFaults)
					{
						AddStateForDataContractSerializerImport(wsdlImporter, codeGeneratorContext);
						return;
					}
					AddStateForFaultSerializerImport(wsdlImporter);
					return;
			}
		}

		private void AddStateForFaultSerializerImport(MetadataImporter wsdlImporter)
		{
			AddState(wsdlImporter, faultImportOptionsBuilder.Build());
		}

		private void AddStateForDataContractSerializerImport(MetadataImporter wsdlImporter, ICodeGeneratorContext codeGeneratorContext)
		{
			AddState(wsdlImporter, xsdDataContractImporterBuilder.Build(codeGeneratorContext));
			AddState(wsdlImporter, wrappedOptionsBuilder.Build(codeGeneratorContext.CodeGeneratorOptions));
		}

		private void AddStateForXmlSerializerImport(MetadataImporter wsdlImporter, ICodeGeneratorContext codeGeneratorContext)
		{
			AddState(wsdlImporter, xmlSerializerImportOptionsBuilder.Build(codeGeneratorContext));
			AddState(wsdlImporter, wrappedOptionsBuilder.Build(codeGeneratorContext.CodeGeneratorOptions));
		}

		private static void AddState<T>(MetadataImporter wsdlImporter, T state) where T : class
		{
			if (!wsdlImporter.State.ContainsKey(typeof(T)))
			{
				wsdlImporter.State.Add(typeof(T), state);
			}
		}
	}
}