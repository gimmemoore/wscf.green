using System;
using System.Runtime.Serialization;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an error occurs while generating the WSDL file.
    /// </summary>
    /// <remarks>This class is serializable.</remarks>
    [Serializable]
    public class WsdlGenerationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WsdlGenerationException class.
        /// </summary>
        public WsdlGenerationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WsdlGenerationException class with a specified 
        /// error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public WsdlGenerationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WsdlGenerationException class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception. If the innerException parameter is not a 
        /// null reference, the current exception is raised in a catch block that handles the inner exception.
        /// </param>
        public WsdlGenerationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WsdlGenerationException class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The object that holds the serialized object data.</param>
        /// <param name="serializationContext">
        /// The contextual information about the source or destination. 
        /// </param>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected WsdlGenerationException(SerializationInfo serializationInfo, StreamingContext serializationContext) : base(serializationInfo, serializationContext)
        {
        }

        #endregion	
		
    }
}