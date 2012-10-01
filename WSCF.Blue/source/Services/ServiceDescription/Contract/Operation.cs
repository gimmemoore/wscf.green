using System.Collections.Generic;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents the content of an action supported by the XML web service.
    /// </summary>
    public class Operation
    {
        #region Private fields

    	private List<Message> messagesCollection;

    	#endregion

        #region Public properties

    	/// <summary>
    	/// Gets or sets the name of the Operation.
    	/// </summary>
    	public string Name { get; set; }

    	/// <summary>
    	/// Gets or sets the documentation text of the Operation.
    	/// </summary>
    	public string Documentation { get; set; }

    	/// <summary>
    	/// Gets or sets the message exchange pattern ( <see cref="Mep"/>) of the operation.
    	/// </summary>
    	public Mep Mep { get; set; }

    	/// <summary>
        /// Gets or sets the  <see cref="MessagesCollection"/> exchanged by the operation.
        /// </summary>
        /// <remarks>
        /// Typically this messages collection contains an in bound message, optional out bound message 
        /// and header messages.
        /// </remarks>
		public List<Message> MessagesCollection
        {
            get { return messagesCollection ?? (messagesCollection = new List<Message>()); }
        }

    	/// <summary>
    	/// Gets or sets the input  <see cref="Message"/> for the instance of Operation class.
    	/// </summary>
    	public Message Input { get; set; }

    	/// <summary>
    	/// Gets or sets the output <see cref="Message"/> for the instance of Operation class.
    	/// </summary>
    	public Message Output { get; set; }

    	#endregion
    }
}