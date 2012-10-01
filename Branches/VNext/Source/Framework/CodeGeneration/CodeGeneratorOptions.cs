using System.Collections.Generic;
using System.ComponentModel;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	/// <summary>
	/// The options that can be applied to the code generation process.
	/// </summary>
	public class CodeGeneratorOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGeneratorOptions"/> class.
		/// </summary>
		public CodeGeneratorOptions()
		{
			UseXmlSerializerForFaults = true;
			NamespaceMappings = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the code generator mode.
		/// </summary>
		public CodeGeneratorMode CodeGeneratorMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="SerializableAttribute"/> is applied to the generated types.
		/// </summary>
		public bool SerializableAttribute { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the generated types will have the internal access modifier applied.
		/// </summary>
		public bool InternalTypes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether all XML schema types will be imported.
		/// </summary>
		public bool ImportXmlTypes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether XML serializable members are ordered using the Order property.
		/// </summary>
		public bool OrderMembers { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether primitive types are generated as properties.
		/// </summary>
		public bool GenerateProperties { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the generated types should implement <see cref="INotifyPropertyChanged"/>.
		/// </summary>
		public bool EnableDataBinding { get; set; }

		/// <summary>
		/// Gets or sets the language for the generated code.
		/// </summary>
		public CodeLanguage CodeLanguage { get; set; }

		/// <summary>
		/// Gets or sets the serializer that will be targeted.
		/// </summary>
		public SerializerMode Serializer { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether special-casing is used for document-wrapped-literal styled documents when parameters are unwrapped.
		/// </summary>
		public bool Wrapped { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use the XML serializer for faults.
		/// </summary>
		public bool UseXmlSerializerForFaults { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to generate asynchronous methods for operations.
		/// </summary>
		public bool AsyncMethods { get; set; }

		/// <summary>
		/// Gets or sets the target .NET Framework version.
		/// </summary>
		public TargetFrameworkVersion TargetFrameworkVersion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to generate message contracts.
		/// </summary>
		public bool TypedMessages { get; set; }

		/// <summary>
		/// Gets or sets the namespace mappings.
		/// </summary>
		public Dictionary<string, string> NamespaceMappings { get; set; }
	}
}
