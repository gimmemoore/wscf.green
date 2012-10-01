namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents the contents of Schema Element in a XML document.
    /// </summary>
    public class SchemaElement
    {
        #region Private fields

        private string elementNamespace;
        private string elementName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a SchemaElement class.
        /// </summary>
        public SchemaElement()
        {
        }
		
        /// <summary>
        /// Initializes a new instance of a SchemaElement class with the specified values.
        /// </summary>
        /// <param name="elementNamespace">Namespace name of the schema element.</param>
        /// <param name="elementName">Name of the schema element.</param>
        public SchemaElement(string elementNamespace, string elementName)
        {
            this.elementNamespace = elementNamespace;
            this.elementName = elementName;
        }

        #endregion


        #region Public properties

        /// <summary>
        /// Gets or sets the namespace name of the SchemaElement.
        /// </summary>
        public string ElementNamespace
        {
            get { return this.elementNamespace; }
            set { this.elementNamespace = value; }
        }

        /// <summary>
        /// Gets or sets the name of the SchemaElement.
        /// </summary>
        public string ElementName
        {
            get { return this.elementName; }
            set { this.elementName = value; }
        }

        #endregion

        #region Operator overloadings
		
        /// <summary>
        /// Compares two InterfaceContract objects for equality.
        /// </summary>
        /// <param name="left">L value of the comparison operator.</param>
        /// <param name="right">R value of the comparison operator.</param>
        /// <returns>A Boolean value indicating whether the two instances are equal or not.</returns>
        public static bool operator ==(SchemaElement left, SchemaElement right)
        {
            return(left.ElementName.ToLower() == right.ElementName.ToLower() && 
                   ((left.ElementNamespace == null && right.ElementNamespace == null) || 
                    left.ElementNamespace.ToLower() == right.ElementNamespace.ToLower()));
        }
		
        /// <summary>
        /// Compares two InterfaceContract objects for inequality.
        /// </summary>
        /// <param name="left">L value of the comparison operator.</param>
        /// <param name="right">R value of the comparison operator.</param>
        /// <returns>A Boolean value indicating whether the two instances are unequal or not.</returns>
        public static bool operator !=(SchemaElement left, SchemaElement right)
        {
            return(!(left.ElementName.ToLower() == right.ElementName.ToLower() &&
                     ((left.ElementNamespace == null && right.ElementNamespace == null) ||
                      left.ElementNamespace.ToLower() == right.ElementNamespace.ToLower())));
        }
		
        #endregion

        #region Overloaded functions
		
        /// <summary>
        /// Overrides the default Equals function. This function checks whether a given object is 
        /// equal to the current instance.
        /// </summary>
        /// <param name="obj">Object to compare with current instance.</param>
        /// <returns>A Boolean value indicating whether the objects are equal or not.</returns>
        public override bool Equals(object obj)
        {			
            return (this == (SchemaElement)obj);
        }

        /// <summary>
        /// Overrides the default GetHashCode() function. This function builds the hash code for 
        /// this instance by combining all the hash codes of private fields of the current instance.
        /// </summary>
        /// <returns>An integer value indicating the Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.ElementName.GetHashCode() ^ this.ElementNamespace.GetHashCode();
        }

        #endregion

    }
}