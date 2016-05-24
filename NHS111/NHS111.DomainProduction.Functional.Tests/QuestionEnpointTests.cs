using System;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using NHS111.Utils.Helpers;
using NUnit.Framework;

namespace NHS111.Domain.Functional.Tests
{
    [TestFixture]
    public class QuestionEndpointTests
    {
        private string _domainProductionApiDomain =
            "https://microsoft-apiapp801c0be2851f4a5a813f3848b7e64ab4.azurewebsites.net/";

        private string _testQuestionId = "PW756.0";
        private string _testPathwayId = "P130";
        private string _testPathwayNo = "PW1401";
        private string _expectedNextId = "PW756.300";

        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>
        /// Example test method for a HTTP GET.
        /// </summary>
        // Question Controller tests.
        [Test]
        public async void TestGetQuestion_returns_valid_response()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainProductionApiDomain + getQuestionEndpoint, _testQuestionId));


            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("\"id\":\"" + _testQuestionId + "\""));
        }

        [Test]
        public async void TestGetQuestion_returns_valid_fields()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainProductionApiDomain + getQuestionEndpoint, _testQuestionId));

            //this checks a responce is returned.
            Assert.IsNotNull(result);
            //these check the right fields are returned.
            Assert.IsTrue(result.Contains("\"id\":\"" + _testQuestionId + "\""));
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            //this next one checks the right question has returned.
            Assert.IsTrue(result.Contains("\"questionNo\":\"Tx1506"));
        }

  

        [Test]
        public async void TestGetQuestion_returns_valid_answers()
        {
            var getQuestionEndpoint = "questions/{0}/answers";
            var result = await _restfulHelper.GetAsync(String.Format(_domainProductionApiDomain + getQuestionEndpoint, _testQuestionId));

            //this checks a responce is returned.
            Assert.IsNotNull(result);

            //these check the right fields are returned.
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));

            //these check the wrong fields are not returned.
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right answers have returned.
            Assert.IsTrue(result.Contains("\"title\":\"Yes"));
            Assert.IsTrue(result.Contains("\"title\":\"I'm not sure"));
            Assert.IsTrue(result.Contains("\"title\":\"No"));
        }

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=55&gender=Male";
            var result = await _restfulHelper.GetAsync(String.Format(_domainProductionApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestGetNextQuestion()
        {
            var getNextQuestionEndpoint = "questions/{0}/answersNext";
            var expectedNextId = "PW756.300";
            var address = String.Format(_domainProductionApiDomain + getNextQuestionEndpoint, _testQuestionId);
            
            System.Net.ServicePointManager.Expect100Continue = false;
            var result =
                await _restfulHelper.PostAsync(address, CreateHTTPRequest("yes"));

            var resultContent = await result.Content.ReadAsStringAsync();
            
            Assert.IsNotNull(result);

            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(resultContent.Contains("\"id\":\"" + expectedNextId + "\""));
            AssertValidResponseSchema(resultContent, ResponseSchemaType.Question);

            //these check the wrong fields are not returned
            Assert.IsFalse(resultContent.Contains("\"maximumAgeExclusive"));
            Assert.IsFalse(resultContent.Contains("\"module"));
            Assert.IsFalse(resultContent.Contains("\"symptomGroup"));

        }
        [Test]
        //follow on for previous test, to ensure next questionID is valid
        public async void TestGetQuestion_returns_expected_Next_Question()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainProductionApiDomain + getQuestionEndpoint, _expectedNextId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id\":\"" + _expectedNextId + "\""));
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"questionNo\":\"Tx1488"));
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
            Answer
        }

        private static void AssertValidResponseSchema(string result, ResponseSchemaType schemaType)
        {
            switch (schemaType)
            {
                case ResponseSchemaType.Pathway:
                    AssertValidPathwayResponseSchema(result);
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
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"Question"));
            Assert.IsTrue(result.Contains("\"group"));
            Assert.IsTrue(result.Contains("\"order"));
            Assert.IsTrue(result.Contains("\"topic"));
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
