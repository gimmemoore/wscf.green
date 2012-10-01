using System;
using System.Threading;

namespace Thinktecture.Wscf.Framework
{
	/// <summary>
	/// Extension methods applied to <see cref="Exception"/> instances.
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Determines whether the specified exception is fatal.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if the specified exception is fatal; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFatal(this Exception exception)
		{
			return ((((exception is OutOfMemoryException) || (exception is ThreadAbortException)) || (exception is StackOverflowException)) || (exception is AccessViolationException));
		}
	}
}
