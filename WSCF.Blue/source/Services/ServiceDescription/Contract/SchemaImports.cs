using System.Collections;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents a collection of <see cref="SchemaImport"/> objects.
    /// </summary>
    public class SchemaImports : CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of a SchemaImports class.
        /// </summary>
        public SchemaImports()
        {
        }

        #endregion
		
        #region Public methods

        /// <summary>
        /// Adds the specified <see cref="SchemaImport"/> to the end of the SchemaImports collection. 
        /// </summary>
        /// <param name="value">The <see cref="SchemaImport"/> to add to the collection.</param>
        /// <returns>The zero-based index where the value parameter has been added.</returns>
        public int Add(SchemaImport value)
        {
            return this.InnerList.Add(value);
        }
		
        #endregion

        #region Public properties

        /// <summary>
        /// Property indexer for the SchemaImports class. Gets or sets the value of a <see cref="SchemaImport"/>
        /// at the specified zero-based index.
        /// </summary>
        public SchemaImport this[int index]
        {
            get { return (SchemaImport)this.InnerList[index]; }
            set { this.InnerList[index] = value; }
        }
		
        #endregion
    }
}