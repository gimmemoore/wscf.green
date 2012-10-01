using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Thinktecture.Tools.Wscf.Services.ServiceDescription.Tests
{
    /// <summary>
    /// Summary description for MetadataFactoryTests
    /// </summary>
    [TestFixture]
    public class MetadataFactoryTests
    {
        /*
         * (1)	Download the metadata and reconstruct the wsdl to return an 
         * interface contract
         * 
         * (2)	Dynamically generate a flat interface contract 
         * 
         * (3)	Reconstruct existing service - / Contract Inference : for a given set 
            of Urls, WSCF downloads the metadata and allows the user to regenerate 
            services with a single set of data contracts.

         * 
         * */

        [Test]
        public void ShouldDownloadMetadataForRemoteUrl()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void ShouldMergeContractAndProduceFlatWsdl()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void ShouldReconstructExistingService()
        {
            throw new NotImplementedException();
        }
    }
}
