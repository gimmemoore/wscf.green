using System.Collections.Generic;
using System.ServiceModel.Description;

using NUnit.Framework;

namespace Thinktecture.Wscf.Framework.CodeGeneration
{
	[TestFixture]
	public class CodeGenerationExceptionTests
	{
		[Test]
		public void MessagesPropertyContainsErrorsProvidedToConstructor()
		{
			MetadataConversionError error1 = new MetadataConversionError("Message1");
			MetadataConversionError error2 = new MetadataConversionError("Message2");
			List<MetadataConversionError> conversionErrors = new List<MetadataConversionError>{error1, error2};
			CodeGenerationException exception = new CodeGenerationException(conversionErrors);

			Assert.That(exception.Messages, Has.Count.EqualTo(2));
			Assert.That(exception.Messages, Has.Member("Message1"));
			Assert.That(exception.Messages, Has.Member("Message2"));
		}
	}
}
