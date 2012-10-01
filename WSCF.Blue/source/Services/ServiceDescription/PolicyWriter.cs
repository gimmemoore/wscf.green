using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.Xml;
using System.IO;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription
{
    internal sealed class PolicyWriter
    {

        #region "Public methods"
        public static string UpdateWsdlWithPolicyInfo(string wsdlData, List<ServiceEndpoint> endpoints, string interfaceName)
        {
            // load the input wsdl info into an XmlDocument so we can append and prepend
            // the policy nodes as appropriate
            XmlDocument inputWsdlDoc = new XmlDocument();
            inputWsdlDoc.LoadXml(wsdlData);

            // Export the endpoints and related bindings
            XmlDocument wcfWsdlDoc = ExportEndpoints(endpoints);


            XmlNamespaceManager nsMgr = new XmlNamespaceManager(wcfWsdlDoc.NameTable);
            nsMgr.AddNamespace("wsp", Constants.NsWsp);
            nsMgr.AddNamespace("wsdl", Constants.NsWsdl);
            nsMgr.AddNamespace("wsu", Constants.NsWsu);

            XmlNodeList policyNodes = wcfWsdlDoc.DocumentElement.SelectNodes("wsp:Policy", nsMgr);

            // Process bottom-up to preserve the original order.
            for (int i = policyNodes.Count - 1; i >= 0; i--)
            {
                XmlNode policyNode = policyNodes[i];
                XmlAttribute IdAttribute = policyNode.Attributes["Id", Constants.NsWsu];
                IdAttribute.Value = IdAttribute.Value.Replace(Constants.InternalContractName, interfaceName);
                XmlNode importedNode = inputWsdlDoc.ImportNode(policyNode, true);
                inputWsdlDoc.DocumentElement.PrependChild(importedNode);
            }

            XmlNodeList bindingNodes = wcfWsdlDoc.DocumentElement.SelectNodes("wsdl:binding", nsMgr);
            for (int i = bindingNodes.Count - 1; i >= 0; i--)
            {
                XmlNode bindingNode = bindingNodes[i];
                XmlNode policyRef = bindingNode.SelectSingleNode("wsp:PolicyReference", nsMgr);
                if (policyRef != null)
                {
                    policyRef.Attributes["URI"].Value = policyRef.Attributes["URI"].Value.Replace(Constants.InternalContractName, interfaceName);
                    string xPath = string.Format("wsdl:binding[@name=\"{0}\"]",
                        bindingNode.Attributes["name"].Value.Replace(Constants.InternalContractName, interfaceName));

                    XmlNode ourBindingNode = inputWsdlDoc.DocumentElement.SelectSingleNode(xPath, nsMgr);
                    XmlNode importedNode = inputWsdlDoc.ImportNode(policyRef, true);
                    ourBindingNode.PrependChild(importedNode);
                }
            }

            // finally return the string contents of the processed xml file
            return inputWsdlDoc.OuterXml;

        }

     
        
        #endregion



        #region "private helpers
        private static XmlDocument ExportEndpoints(List<ServiceEndpoint> endpoints)
        {
            WsdlExporter exporter = new WsdlExporter();
            foreach (ServiceEndpoint ep in endpoints)
            {
                exporter.ExportEndpoint(ep);
            }

            MetadataSet metadataSet = exporter.GetGeneratedMetadata();
            StringBuilder b = new StringBuilder();
            StringWriter sw = new StringWriter(b);
            XmlTextWriter tw = new XmlTextWriter(sw);
            foreach (MetadataSection section in metadataSet.MetadataSections)
            {
                if (section.Metadata is System.Web.Services.Description.ServiceDescription)
                {
                    System.Web.Services.Description.ServiceDescription sd = (System.Web.Services.Description.ServiceDescription)section.Metadata;
                    sd.Write(tw);
                }
            }

            string wcfWsdl = b.ToString();
            // Read it in to an XmlDocument.
            XmlDocument wcfWsdlDoc = new XmlDocument();
            wcfWsdlDoc.LoadXml(wcfWsdl);
            return wcfWsdlDoc;
        }

        #endregion

    }
}
