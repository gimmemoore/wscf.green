using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription.Helpers
{
    internal static class SchemaUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static ArrayList GetIntrinsicSimpleTypesNames()
        {
            ArrayList primitiveNames = new ArrayList();
            Assembly assembly = Assembly.GetAssembly(typeof(XmlSchema));

            Type type = assembly.GetType("System.Xml.Schema.DatatypeImplementation");
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            foreach (FieldInfo fi in fields)
            {
                int index = fi.Name.IndexOf("c_");
                if (index > -1)
                {
                    string fieldName = fi.Name.Substring(index + 2);
                    primitiveNames.Add("xsd:" + fieldName);
                }
            }

            return primitiveNames;
        }

        /// <summary>
        /// Reads a XML schema file and returns the information found in that.
        /// </summary>
        /// <param name="schemaFile">The XML schema file to read information from.</param>
        /// <param name="schemaNamespace">Ouput parameter which returns the namespace of the specified XML schema file.</param>
        /// <returns>
        /// An <see cref="ArrayList"/> with three items. 
        /// 1. Contains an <see cref="ArrayList"/> of <see cref="XmlSchemaElement"/> objects.
        /// 2. Contains an <see cref="ArrayList"/> of schema element names.
        /// 3. Contains a <see cref="SchemaElements"/> object. 
        /// </returns>
        internal static ArrayList GetSchemasFromXsd(string schemaFile, out string schemaNamespace)
        {
            XmlTextReader reader = null;
            ArrayList schemas;
            ArrayList schemaNames;
			List<SchemaElement> sElements;

            try
            {
                reader = new XmlTextReader(schemaFile);

                XmlSchema schema = XmlSchema.Read(reader, null);
                string schemaTargetNamesapce = schema.TargetNamespace;
                schemaNamespace = schemaTargetNamesapce;

                ArrayList xmlSchemaElements = new ArrayList();
                schemas = new ArrayList();
                schemaNames = new ArrayList();
				sElements = new List<SchemaElement>();

                foreach (XmlSchemaObject xmlObj in schema.Items)
                {
                    if (xmlObj is XmlSchemaAnnotated) xmlSchemaElements.Add(xmlObj);
                }

                foreach (XmlSchemaAnnotated obj in xmlSchemaElements)
                {
                    if (obj is XmlSchemaElement)
                    {
                        XmlSchemaElement xse = (XmlSchemaElement)obj;

                        schemas.Add(xse);
                        schemaNames.Add(xse.Name);
                        sElements.Add(new SchemaElement(schemaTargetNamesapce, xse.Name));
                    }
                }

                reader.Close();

                ArrayList result = new ArrayList();
                result.Add(schemas);
                result.Add(sElements);
                result.Add(schemaNames);

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while reading the schema file.", ex);
            }
            finally
            {
                if (reader != null && reader.ReadState != ReadState.Closed)
                {
                    reader.Close();
                }
            }
        }
    }
}
