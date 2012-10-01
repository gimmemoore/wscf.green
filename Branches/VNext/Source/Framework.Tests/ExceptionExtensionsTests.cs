using System;

using NUnit.Framework;

namespace Thinktecture.Wscf.Framework
{
	[TestFixture]
	public class ExceptionExtensionsTests
	{
		[Test]
		public void IsFatal_Exception_IsNotFatal()
		{
			Assert.That(new Exception().IsFatal(), Is.False);
		}

		[Test]
		public void IsFatal_OutOfMemoryException_IsFatal()
		{
			Assert.That(new OutOfMemoryException().IsFatal(), Is.True);
		}

		[Test]
		public void IsFatal_StackOverflowException_IsFatal()
		{
			Assert.That(new StackOverflowException().IsFatal(), Is.True);
		}

		[Test]
		public void IsFatal_AccessViolationException_IsFatal()
		{
			Assert.That(new AccessViolationException().IsFatal(), Is.True);
		}
	}
}