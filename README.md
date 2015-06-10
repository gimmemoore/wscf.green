#WSCF.Green#
WSCF.Green is a fork from the Thinktecture WSCF.Blue.

##Description##
**WSCF.blue** is a toolset that facilitates the development of web services using a contract first (specifically, a schema first) approach. This is the WCF version of the orginal [Web Services Contract First tool](http://www.thinktecture.com/WSCF).

##Walkthrough Document & Request For Feature Votes##
We have published a [walkthrough document](http://wscfblue.codeplex.com/releases/view/48529). This applies to both VS2008 and VS2010. Check it out and send us feedback on the content and suggestions for any additional content.
nVext planning/work is underway but we need to know what you would like to see. The main items on our roadmap have now been entered into the Issue Tracker (and tracked against a "Roadmap" release). Vote on these and tell us what features would be the most useful to you. If there are others that we havent listed, feel free to add them into the issue tracker.

##Introduction#
When developing Web Services, the first step is usually to:

- Design your contract's data, messages and interface
- Generate code from the contract

This first step can be done either in code or can be done with XML and XSD. As it turns out a number of enterprise scale projects prefer to take the option of starting with XML and XSD. For many integration and application development scenarios (not only at the enterprise level) it is customary to negotiate a WSDL/XSD based specification for the web services and then to embark on the actual development of the code that implements that specification. However, dealing with raw XML and WSDL can be very error-prone. WSDL in particular is non-trivial to handle as the original WSDL specification allows room for some complicated constructs and contracts to be defined. We need tools that can enable us to work at this level consistently and reliably and **WSCF** aims to address this need.

The screenshot below shows the main steps involved in schema-first web service development. For more information please read the [Schema-based Development with Windows Communication Foundation](http://msdn.microsoft.com/en-us/magazine/ee335699.aspx) article on the MSDN Magazine website.


![](http://i.imgur.com/yQqTF4n.png)

##The WSCF.blue tool##
**WSCF.blue** is a Visual Studio .NET 2008/2010 add-in that provides the following features:

- A **WSDL Wizard** that allows the developer to step through the creation of a WSDL from one or more XSDs.
- A **Data Contract Generator** (similar to XSD.exe, XSDObjectGen.exe and Svcutil.exe) that generates the . NET equivalent of the XSD types.
- A **Service/Endpoint Stub (SVC) Generator** and
- A **Client Proxy Generator**.
- A **Generate Data Contract Code** feature that supports the selection of multiple XSD/WSDL source files. ([More Information](http://alexmg.com/post/2009/09/01/Data-contract-generation-is-now-available-in-WSCFblue.aspx))
- Support for **C#** and **VB.NET** code generation.
- You can choose if operation methods on your service class will throw a NotImplementedException, call an implementation method in a partial class, or will be defined as abstract methods. ([More Information](http://alexmg.com/post/2009/08/08/Controlling-your-Service-method-implementation-in-WSCFblue.aspx))
- Force the SOAP actions (Action and ReplyAction) applied to each operation contract follow the standard WCF format: <namespace>/<service>/<operation>[Response]
- A **Paste XML as Schema** option that generates a schema for a block of XML in the clipboard. ([More Information](http://alexmg.com/post/2009/09/21/Paste-XML-as-Schema-in-WSCFblue.aspx))
- Errors found in your WSDL are reported in a WSCF.blue pane in the Output window. ([More Information](http://alexmg.com/post/2009/09/28/Improved-WSDL-error-handling-in-WSCFblue.aspx))

For the endpoints and the client proxy, WSCF also generates the necessary configuration file. 