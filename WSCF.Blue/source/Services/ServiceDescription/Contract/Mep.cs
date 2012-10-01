namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Defines the available options for the Message Exchange Patterns used by the XML web service.
    /// </summary>
    public enum Mep
    {
        /// <summary>
        /// Indicates the message is a one-way message.
        /// </summary>
        OneWay,
        /// <summary>
        /// Indicates the message is a request/response (two-way) message.
        /// </summary>
        RequestResponse
    }
}