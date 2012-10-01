namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents the content of the Message headers exchanged by the XML web service.
    /// </summary>
    public class MessageHeader
    {
        #region Private fields

        private string name;
        private string message;
        private string part;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the name of the MessageHeader.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Message for the instance of MessageHeader.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets or sets the message part of the instance of the MessageHeader.
        /// </summary>
        public string Part
        {
            get { return part; }
            set { part = value; }
        }

        #endregion
    }
}