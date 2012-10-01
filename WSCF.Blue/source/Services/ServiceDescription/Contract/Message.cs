using System.Collections.Generic;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents the content of the Message exchanged by XML web service.
    /// </summary>
    public class Message
    {
        #region Private fields.

    	private List<MessageHeader> headersCollection;
		
        #endregion

        #region Constructors
		
        /// <summary>
        /// Initializes a new instance of the Message class.
        /// </summary>
        public Message()
        {
            Element = new SchemaElement();
        }

        #endregion
		
        #region Public properties
        /// <summary>
        /// Gets or sets the MessageHeader(s) for the current instance of the Message.
        /// </summary>
		public List<MessageHeader> HeadersCollection
        {
            get { return headersCollection ?? (headersCollection = new List<MessageHeader>()); }
        }

    	/// <summary>
    	/// Gets or sets the name of the Message.
    	/// </summary>
    	public string Name { get; set; }

    	/// <summary>
    	/// Gets or sets the SchemaElement of the Message.
    	/// </summary>
    	public SchemaElement Element { get; set; }

    	/// <summary>
    	/// Gets or sets the documentation text of the Message.
    	/// </summary>
    	public string Documentation { get; set; }

    	#endregion
    }
}