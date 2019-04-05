using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
namespace NHS111.Web.Presentation.Configuration.Tests
{
    [TestFixture()]
    public class ConfigurationTests
    {
        Configuration _testConfiguration = new Configuration();

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"] = "http://testbusinessdomain.com/";
        }

        [Test()]
        public void GetBusinessApiGroupedPathwaysUrl_with_absolute_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiGroupedPathwaysUrl"] = "http://testabsoluite.com/someendpoint/{0}";
            var result = _testConfiguration.GetBusinessApiGroupedPathwaysUrl("mysearch");

            Assert.AreEqual("http://testabsoluite.com/someendpoint/mysearch", result);
        }

        [Test()]
        public void GetBusinessApiGroupedPathwaysUrl_with_relative_url_configured_Test()
        {

            ConfigurationManager.AppSettings["BusinessApiGroupedPathwaysUrl"] = "someendpoint/{0}";
            var result = _testConfiguration.GetBusinessApiGroupedPathwaysUrl("mysearch");

            Assert.AreEqual("someendpoint/mysearch", result);
        }

        [Test()]
        public void GetBusinessApiPathwayUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiPathwayUrl"] = "somepathway/endpoint/{0}";
            var result = _testConfiguration.GetBusinessApiPathwayUrl("PW123.4");

            Assert.AreEqual("somepathway/endpoint/PW123.4", result);
        }

        [Test()]
        public void GetBusinessApiPathwayIdUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiPathwayIdUrl"] = "somepathway/endpoint/{0}/{1}/{2}";
            var result = _testConfiguration.GetBusinessApiPathwayIdUrl("PW1234", "male", 22);

            Assert.AreEqual("somepathway/endpoint/PW1234/male/22", result);
        }

        [Test()]
        public void GetBusinessApiPathwaySymptomGroupUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiPathwaySymptomGroupUrl"] = "symptomgroupsendpoint/{0}";
            var result = _testConfiguration.GetBusinessApiPathwaySymptomGroupUrl("test,test2");

            Assert.AreEqual("symptomgroupsendpoint/test,test2", result);
        }

        [Test()]
        public void GetBusinessApiNextNodeUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiNextNodeUrl"] = "nextEndpoint/{0}/{1}/next/{2}/{3}";
                var result = _testConfiguration.GetBusinessApiNextNodeUrl("PW1234", NodeType.Question,  "xxx-ddd", "{State:'somestate'}");

            Assert.AreEqual("nextEndpoint/PW1234/Question/next/xxx-ddd/{State:'somestate'}", result);
        }

        [Test()]
        public void GetBusinessApiQuestionByIdUrll_with_relative_url_configured_TestTest()
        {
            ConfigurationManager.AppSettings["BusinessApiQuestionByIdUrl"] = "questionEndpoint/{0}/next/{1}";
            var result = _testConfiguration.GetBusinessApiQuestionByIdUrl("PW123.4", "Tx123456");

            Assert.AreEqual("questionEndpoint/PW123.4/next/Tx123456", result);
        }

        [Test()]
        public void GetBusinessApiCareAdviceUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiCareAdviceUrl"] = "careadviceEndpoint/{0}/stuff/{1}/?markers={2}";
            var result = _testConfiguration.GetBusinessApiCareAdviceUrl(23, "female", "Cx1234,Cx12345");

            Assert.AreEqual("careadviceEndpoint/23/stuff/female/?markers=Cx1234,Cx12345", result);
        }

        [Test()]
        public void GetBusinessApiFirstQuestionUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiFirstQuestionUrl"] = "firstQuestionEndpoint/{0}/?state={1}";
            var result = _testConfiguration.GetBusinessApiFirstQuestionUrl("PW123.4", "{State:'somestate'}");

            Assert.AreEqual("firstQuestionEndpoint/PW123.4/?state={State:'somestate'}", result);
        }

        [Test()]
        public void GetBusinessApiPathwayNumbersUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiPathwayNumbersUrl"] = "pathwayNumbersEndpoint/{0}";
            var result = _testConfiguration.GetBusinessApiPathwayNumbersUrl("Testpathway");

            Assert.AreEqual("pathwayNumbersEndpoint/Testpathway", result);
        }

        [Test()]
        public void GetBusinessApiPathwayIdFromTitleUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiPathwayIdFromTitleUrl"] = "pathwayIdEndpoint/{0}/{1}/{2}";
            var result = _testConfiguration.GetBusinessApiPathwayIdFromTitleUrl("Testpathway", "male", 44);

            Assert.AreEqual("pathwayIdEndpoint/Testpathway/male/44", result);
        }

        [Test()]
        public void GetBusinessApiJustToBeSafePartOneUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiJustToBeSafePartOneUrl"] = "jtbsFirstEndpoint/{0}";
            var result = _testConfiguration.GetBusinessApiJustToBeSafePartOneUrl("PW123.4");

            Assert.AreEqual("jtbsFirstEndpoint/PW123.4", result);
        }

        [Test()]
        public void GetBusinessApiJustToBeSafePartTwoUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiJustToBeSafePartTwoUrl"] = "jtbsSecondEndpoint/{0}/jtbs/second/{2}/{3}/{1}";
            var result = _testConfiguration.GetBusinessApiJustToBeSafePartTwoUrl("PW123.4", "Tx12345", "Tx3333,Tx444", true);

            Assert.AreEqual("jtbsSecondEndpoint/PW123.4/jtbs/second/Tx3333,Tx444/True/Tx12345", result);
        }

        [Test()]
        public void GetBusinessApiInterimCareAdviceUrl_with_relative_url_configured_Test()
        {
            ConfigurationManager.AppSettings["BusinessApiInterimCareAdviceUrl"] = "pathways/care-adviceEndpointTest/{0}/{1}/{2}";
            var result = _testConfiguration.GetBusinessApiInterimCareAdviceUrl("Dx9999", "Toddler", "Male");

            Assert.AreEqual("pathways/care-adviceEndpointTest/Dx9999/Toddler/Male", result);
       
        }
      
        [Test]
        public void IsPublic_WhenNotDefined_DefaultsToTrue() {
            Assert.IsTrue(_testConfiguration.IsPublic);
        }      
    }
}
