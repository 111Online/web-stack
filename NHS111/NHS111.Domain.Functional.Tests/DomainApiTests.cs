using System;
using System.Configuration;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Domain.Functional.Tests
{
    using NHS111.Functional.Tests.Tools;

    [TestFixture]
    public class DomainApiTests
    {
        private static string DomainApiBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainApiBaseUrl"]; 
            }
        }

        private string _testQuestionId = "PW756.0";
        private string _testPathwayId = "PW756MaleChild";
        private string _testPathwayNo = "PW1708";
        private string _expectedNextId = "PW756.300";

        private IRestClient _restClient = new RestClient(DomainApiBaseUrl);
        private string DxCode = "Dx12";

        /// <summary>
        /// Example test method for a HTTP GET.
        /// </summary>
        // Question Controller tests.
        [Test]
        public async void TestDomainApi_returns_valid_response()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testQuestionId), Method.GET));

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Content.Contains("\"id\":\"" + _testQuestionId + "\""));
        }

        [Test]
        public async void TestDomainApi_returns_valid_fields()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testQuestionId), Method.GET));

            //this checks a responce is returned.
            Assert.IsNotNull(result);
            //these check the right fields are returned.
            Assert.IsTrue(result.Content.Contains("\"id\":\"" + _testQuestionId + "\""));
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned.
            Assert.IsTrue(result.Content.Contains("\"questionNo\":\"TX1506"));
        }

        [Test]
        public async void TestDomainApi_returns_valid_answers()
        {
            var getQuestionEndpoint = "questions/{0}/answers";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testQuestionId), Method.GET));

            //this checks a responce is returned.
            Assert.IsNotNull(result);

            //these check the right fields are returned.
            Assert.IsTrue(result.Content.Contains("\"title"));
            Assert.IsTrue(result.Content.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Content.Contains("\"order"));

            //these check the wrong fields are not returned.
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Answer);

            //this next one checks the right answers have returned.
            Assert.IsTrue(result.Content.Contains("\"title\":\"Yes"));
            Assert.IsTrue(result.Content.Contains("\"title\":\"I'm not sure"));
            Assert.IsTrue(result.Content.Contains("\"title\":\"No"));
        }


        [Test]
        public async void TestDomainApi_returns_valid_first_question()
        {
            var getQuestionEndpoint = "pathways/{0}/questions/first";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayId), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"questionNo\":\"TX1506"));
        }

        // Care Advice Controller tests
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Fields()
        {
            var getQuestionEndpoint = "pathways";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"title\":\"Chest or Upper Back Injury, Blunt"));
            Assert.IsTrue(result.Content.Contains("\"title\":\"Headache"));
            Assert.IsFalse(result.Content.Contains("\"title\":\"Blood in urine"));
        }

        [Test]
        public async void TestDomainApi_returns_valid_Pathway_ID()
        {
            var getQuestionEndpoint = "pathways/{0}";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayId), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Content.Contains("\"pathwayNo\":\"PW756"));
            Assert.IsTrue(result.Content.Contains("\"gender\":\"Male"));
            Assert.IsFalse(result.Content.Contains("\"title\":\"Abdominal Pain"));
        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Male";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayNo), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"title\":\"Diarrhoea and Vomiting"));
            Assert.IsTrue(result.Content.Contains("\"id\":\"PW1708MaleInfant"));
            Assert.IsTrue(result.Content.Contains("\"gender\":\"Male"));
            Assert.IsTrue(result.Content.Contains("\"pathwayNo\":\"PW1708"));

        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers_InvalidAge()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=55&gender=Male";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayNo), Method.GET));

            //this checks a responce is returned
            Assert.IsTrue(result.Content.Contains("null"));


        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers_GenderChange()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Female";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayNo), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"title\":\"Diarrhoea and Vomiting"));
            Assert.IsTrue(result.Content.Contains("\"id\":\"PW1708FemaleInfant"));
            Assert.IsTrue(result.Content.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Content.Contains("\"pathwayNo\":\"PW1708"));

        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathways/symptomGroup/{0}";
            var result = await _restClient.ExecuteTaskAsync<string>(new JsonRestRequest(string.Format(getQuestionEndpoint, _testPathwayNo), Method.GET));

            var response = result.Data;

            //this checks a responce is returned
            Assert.IsNotNull(response);

            //these check the right fields are returned
            Assert.IsTrue(response.Contains("1055"));

            //this checks only the SD code returns
            Assert.AreEqual("", response.Replace("1055", ""));

        }
        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestDomainApi_GetNextQuestion()
        {
            var getNextQuestionEndpoint = "questions/{0}/Question/answersNext";
            var expectedNextId = "PW756.300";
            var url = string.Format(getNextQuestionEndpoint, _testQuestionId);

           var request = new JsonRestRequest(url, Method.POST);
            request.AddJsonBody("Yes");
            var result = await _restClient.ExecuteTaskAsync(request);

            var resultContent = result.Content;

            Assert.IsNotNull(result);

            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(resultContent.Contains("\"id\":\"" + expectedNextId + "\""));
            SchemaValidation.AssertValidResponseSchema(resultContent, SchemaValidation.ResponseSchemaType.Question);

            //these check the wrong fields are not returned
            Assert.IsFalse(resultContent.Contains("\"maximumAgeExclusive"));
            Assert.IsFalse(resultContent.Contains("\"module"));
            Assert.IsFalse(resultContent.Contains("\"symptomGroup"));

        }
        [Test]
        //follow on for previous test, to ensure next questionID is valid
        public async void TestDomainApi_returns_expected_Next_Question()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restClient.ExecuteTaskAsync(new JsonRestRequest(string.Format(getQuestionEndpoint, _expectedNextId), Method.GET));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Content.Contains("\"id\":\"" + _expectedNextId + "\""));
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Content.Contains("\"questionNo\":\"TX1488"));
        }
    }
}
