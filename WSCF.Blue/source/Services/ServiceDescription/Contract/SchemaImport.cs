using System;
using System.IO;
using Thinktecture.Tools.Web.Services.Wscf.Environment;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    /// <summary>
    /// Represents the details of the imported schemas to the WSDL.
    /// </summary>
    public class SchemaImport
    {
        #region Private fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of SchemaImport class.
        /// </summary>
        public SchemaImport()
        {

        }

        /// <summary>
        /// Initializes a new instance of the SchemaImport class with the specified values.
        /// </summary>
        /// <param name="schemaLocation">File location of the imported schema.</param>
        /// <param name="schemaNamespace">Namespace of the imported schema.</param>
        /// <param name="schemaName">Name of the imported schema.</param>
        public SchemaImport(string schemaLocation, string schemaName, string schemaNamespace, string workingFolder, string rootFolder)
        {
            this.SchemaLocation = schemaLocation;
            this.SchemaName = schemaName;
            this.SchemaNamespace = schemaNamespace;
            SetAlternateLocation(workingFolder, rootFolder);
        }

        #endregion
		
        #region Public properties

        /// <summary>
        /// Gets or sets the name of the imported schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the file name of the imported schema.
        /// </summary>
        public string SchemaLocation { get; set; }

        /// <summary>
        /// Gets or sets the namespace of the imported schema.
        /// </summary>
        public string SchemaNamespace { get; set; }


        /// <summary>
        /// Gets or sets the alternate location for the imported schema.
        /// </summary>
        public string AlternateLocation { get; set; }

        #endregion

        private void SetAlternateLocation(string workingFolder, string projectRootFolder)
        {
            // Check whether the schema is in the current directory.
            if (Directory.GetFiles(workingFolder, this.SchemaName).Length > 0)
            {
                AlternateLocation = this.SchemaName;
            }
            else if (
                !String.IsNullOrEmpty(projectRootFolder) && 
                this.SchemaLocation.ToLower().StartsWith(projectRootFolder.ToLower())
                )
            {
                string schemaDirectory = this.SchemaLocation.Substring(
                    0, this.SchemaLocation.LastIndexOf('\\'));
                string currentDirectory = workingFolder;
                // Remove the project root before passing them to the relative path finder.
                schemaDirectory = schemaDirectory.Substring(projectRootFolder.Length);
                currentDirectory = currentDirectory.Substring(projectRootFolder.Length);

                AlternateLocation = IOPathHelper.GetRelativePath(schemaDirectory, currentDirectory);
                if (AlternateLocation.EndsWith("/"))
                {
                    AlternateLocation = AlternateLocation + this.SchemaName;
                }
                else
                {
                    AlternateLocation = AlternateLocation + "/" + this.SchemaName;
                }
            }
            else
            {
                AlternateLocation = this.SchemaLocation;
            }


        }
    }
}