using System;
using System.Configuration;
using NHS111.Utils.Helpers;
using NUnit.Framework;

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

        private RestfulHelper _restfulHelper = new RestfulHelper();
        private string DxCode = "Dx12";

        /// <summary>
        /// Example test method for a HTTP GET.
        /// </summary>
        // Question Controller tests.
        [Test]
        public async void TestDomainApi_returns_valid_response()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testQuestionId));


            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("\"id\":\"" + _testQuestionId + "\""));
        }

        [Test]
        public async void TestDomainApi_returns_valid_fields()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testQuestionId));

            //this checks a responce is returned.
            Assert.IsNotNull(result);
            //these check the right fields are returned.
            Assert.IsTrue(result.Contains("\"id\":\"" + _testQuestionId + "\""));
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned.
            Assert.IsTrue(result.Contains("\"questionNo\":\"TX1506"));
        }

        [Test]
        public async void TestDomainApi_returns_valid_answers()
        {
            var getQuestionEndpoint = "questions/{0}/answers";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testQuestionId));

            //this checks a responce is returned.
            Assert.IsNotNull(result);

            //these check the right fields are returned.
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));

            //these check the wrong fields are not returned.
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Answer);

            //this next one checks the right answers have returned.
            Assert.IsTrue(result.Contains("\"title\":\"Yes"));
            Assert.IsTrue(result.Contains("\"title\":\"I'm not sure"));
            Assert.IsTrue(result.Contains("\"title\":\"No"));
        }


        [Test]
        public async void TestDomainApi_returns_valid_first_question()
        {
            var getQuestionEndpoint = "pathways/{0}/questions/first";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"questionNo\":\"TX1506"));
        }

        // Care Advice Controller tests
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Fields()
        {
            var getQuestionEndpoint = "pathways";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Chest or Upper Back Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsFalse(result.Contains("\"title\":\"Blood in urine"));
        }

        [Test]
        public async void TestDomainApi_returns_valid_Pathway_ID()
        {
            var getQuestionEndpoint = "pathways/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW756"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsFalse(result.Contains("\"title\":\"Abdominal Pain"));
        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Male";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Diarrhoea and Vomiting"));
            Assert.IsTrue(result.Contains("\"id\":\"PW1708MaleInfant"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1708"));

        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers_InvalidAge()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=55&gender=Male";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Numbers_GenderChange()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Female";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Diarrhoea and Vomiting"));
            Assert.IsTrue(result.Contains("\"id\":\"PW1708FemaleInfant"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1708"));

        }
        [Test]
        public async void TestDomainApi_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathways/symptomGroup/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1055"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1055", ""));

        }
        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestDomainApi_GetNextQuestion()
        {
            var getNextQuestionEndpoint = "questions/{0}/answersNext";
            var expectedNextId = "PW756.300";
            var address = String.Format(DomainApiBaseUrl + getNextQuestionEndpoint, _testQuestionId);

            System.Net.ServicePointManager.Expect100Continue = false;
            var result =
                await _restfulHelper.PostAsync(address, RequestFormatting.CreateHTTPRequest("Yes"));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsNotNull(result);

            Assert.IsTrue(result.IsSuccessStatusCode);
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
            var result = await _restfulHelper.GetAsync(String.Format(DomainApiBaseUrl + getQuestionEndpoint, _expectedNextId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id\":\"" + _expectedNextId + "\""));
            SchemaValidation.AssertValidResponseSchema(result, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"questionNo\":\"TX1488"));
        }
    }
}
