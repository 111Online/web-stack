﻿
namespace NHS111.Business.API.Functional.Tests
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Text;
    using Utils.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class QuestionEndpointTests
    {
        private string _BusinessdomainApiDomain =
            "https://microsoft-apiapp40f6723d48db47ed8f4d3ff1-integration.azurewebsites.net/";

        private string _testQuestionId = "PW1346.1000";
        private string _testPathwayNo2 = "PW752";
        private string _testPathwayNo = "PW1708";
        private string _expectedNodeId = "PW752.200";
        private  string DxCode1 = "Dx12";

        private RestfulHelper _restfulHelper = new RestfulHelper();

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW752"));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge1()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/1";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge200()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/200";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge15()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/15";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidGender()
        {
            var getQuestionEndpoint = "pathway/{0}/Male/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathway/symptomGroup/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1055"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1055", ""));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway()
        {
            var getQuestionEndpoint = "pathway";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

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

        public static HttpRequestMessage CreateHTTPRequest(string requestContent)
        {
            return new HttpRequestMessage
            {
                Content = new StringContent("\"" + requestContent + "\"", Encoding.UTF8, "application/json")
            };
        }


        public enum ResponseSchemaType
        {
            Pathway,
            Question,
            Answer,
            Answers
        }

        private static void AssertValidResponseSchema(string result, ResponseSchemaType schemaType)
        {
            switch (schemaType)
            {
                case ResponseSchemaType.Pathway:
                    AssertValidPathwayResponseSchema(result);
                    break;
                case ResponseSchemaType.Answers:
                    AssertValidAnswersResponseSchema(result);
                    break;
                case ResponseSchemaType.Question:
                    AssertValidQuestionResponseSchema(result);
                    break;
                case ResponseSchemaType.Answer:
                    AssertValidAnswerResponseSchema(result);
                    break;
                default:
                    throw new InvalidEnumArgumentException("ResponseSchemaType of " + schemaType.ToString() +
                                                       "is unsupported");
            }

        }



        private static void AssertValidAnswerResponseSchema(string result)
        {
            Assert.IsFalse(result.Contains("\"Question"));
            Assert.IsFalse(result.Contains("\"group"));
            Assert.IsFalse(result.Contains("\"topic"));
            Assert.IsFalse(result.Contains("\"questionNo"));
            Assert.IsFalse(result.Contains("\"jtbs"));
            Assert.IsFalse(result.Contains("\"jtbsText"));
            Assert.IsFalse(result.Contains("\"Answers"));
            Assert.IsFalse(result.Contains("\"Labels"));
        }

        private static void AssertValidAnswersResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));

        }
        private static void AssertValidPathwayResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"pathwayNo"));
            Assert.IsTrue(result.Contains("\"gender"));
            Assert.IsTrue(result.Contains("\"minimumAgeInclusive"));
            Assert.IsTrue(result.Contains("\"maximumAgeExclusive"));
            Assert.IsTrue(result.Contains("\"module"));
            Assert.IsTrue(result.Contains("\"symptomGroup"));
            Assert.IsTrue(result.Contains("\"group"));
        }

        private static void AssertValidQuestionResponseSchema(string result)
        {

            Assert.IsTrue(result.Contains("\"Question"));
            Assert.IsTrue(result.Contains("\"group"));
            Assert.IsTrue(result.Contains("\"order"));
            Assert.IsTrue(result.Contains("\"topic"));
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"questionNo"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"jtbs"));
            Assert.IsTrue(result.Contains("\"jtbsText"));
            Assert.IsTrue(result.Contains("\"Answers"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"Labels"));
            Assert.IsTrue(result.Contains("\"State"));
        }

    }
}
