

namespace NHS111.Business.API.Functional.Tests
{
    using System;
    using System.Net.Http;
    using System.Text;
    using NUnit.Framework;
    using System.Configuration;
    using Utils.Helpers;
    using NHS111.Functional.Tests.Tools;

    [TestFixture]
    public class BusinessApiTests
    {
        private string _testQuestionId = "PW1346.1000";
        private string _testPathwayNo2 = "PW752";
        private string _testPathwayNo = "PW1708";
        private string _expectedNodeId = "PW752.200";
        private  string DxCode1 = "Dx12";

        private RestfulHelper _restfulHelper = new RestfulHelper();

        private static string BusinessApiPathwayUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiPathwayUrl"]); }
        }

        private static string BusinessApiPathwaySymptomGroupUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiPathwaySymptomGroupUrl"]); }
        }

        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "/Female/16";
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW752"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidAge1()
        {
            var getQuestionEndpoint = "/Female/1";
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidAge200()
        {
            var getQuestionEndpoint = "/Female/200";
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidAge15()
        {
            var getQuestionEndpoint = "/Female/15";
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidGender()
        {
            var getQuestionEndpoint = "/Male/16";
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }

        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Symptom_Group()
        {
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwaySymptomGroupUrl, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1055"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1055", ""));

        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway()
        {
            var result = await _restfulHelper.GetAsync(String.Format(BusinessApiPathwayUrl, string.Empty));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW753"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW756"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW752"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW755"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW754"));
        }
    }
}
