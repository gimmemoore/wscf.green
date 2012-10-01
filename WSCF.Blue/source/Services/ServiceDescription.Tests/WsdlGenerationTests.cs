using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription.Tests
{

    /*
     * Wsdl Generation
        (1)	Pass in a set of schemas to the SDBuilder 
        (2)	Choose InferOperations to get a list of operations and their message exchange patterns or enter in operationMessageMap which has operation, mep, message and associated header and documentation
        (3)	Generate the service contract
     * */
    /// <summary>
    /// Summary description for WsdlGenerationTests
    /// </summary>
    [TestFixture]
    public class WsdlGenerationTests
    {
        #region "contract assertion tests"
        // Tests to prove that Service DescriptionBuilder will verify correctness of 
        // all input parameters
        [Test]
        public void ExpectExceptionForNullSchemaSets()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region "main behavior tests"
        
        [Test]
        public void ShouldInferOneWayOperationNames()
        {
            //
            // TODO: Add test logic	here
            //
        }

        [Test]
        public void ShouldInferTwoWayOperationNames()
        {
            

        }


        [Test]
        public void ShouldInferOtherMepOperationNames()
        {


        }
        #endregion

        #region "Wsdl Generation  tests"
        // contains tests to prove that WSDLs are generated correctly
        // with advanced settings such as Soap12 binding, WSA etc. 

        [Test]
        public void ShouldGenerateWsdlWithSoap11Binding()
        {


        }

        #endregion

    }
}
