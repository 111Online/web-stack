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
        private string _domainApiDomain =
            "http://microsoft-apiapp801c0be2851f4a5a813f3848-integration.azurewebsites.net/";

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
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testQuestionId));


            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("\"id\":\"" + _testQuestionId + "\""));
        }

        [Test]
        public async void TestGetQuestion_returns_valid_fields()
        {
            var getQuestionEndpoint = "questions/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testQuestionId));

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
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testQuestionId));

            //this checks a responce is returned.
            Assert.IsNotNull(result);

            //these check the right fields are returned.
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));

            //these check the wrong fields are not returned.
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right answers have returned.
            Assert.IsTrue(result.Contains("\"title\":\"yes"));
            Assert.IsTrue(result.Contains("\"title\":\"not sure"));
            Assert.IsTrue(result.Contains("\"title\":\"no"));
        }


        [Test]
        public async void TestGetQuestion_returns_valid_first_question()
        {
            var getQuestionEndpoint = "pathways/{0}/questions/first";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"questionNo\":\"Tx100077"));
        }
        // Care Advice Controller tests
        [Test]
        public async void TestGetQuestion_returns_valid_care_advice_AdultAge()
        {
            var getQuestionEndpoint = "pathways/care-advice/43/Female?markers=Cx220179";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"excludeTitle"));
            Assert.IsTrue(result.Contains("\"items"));

            //these check the wrong fields are not returned
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"id\":\"Cx220179-Adult-Female"));
            Assert.IsTrue(result.Contains("\"title\":\"Needlestick injury"));
        }
        [Test]
        public async void TestGetQuestion_returns_valid_care_advice_ToddlerAge()
        {
            var getQuestionEndpoint = "pathways/care-advice/1/Female?markers=Cx220179";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"excludeTitle"));
            Assert.IsTrue(result.Contains("\"items"));

            //these check the wrong fields are not returned
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right question has returned
            Assert.IsFalse(result.Contains("\"id\":\"Cx220179-Adult-Female"));
            Assert.IsTrue(result.Contains("\"id\":\"Cx220179-Toddler-Female"));
            Assert.IsTrue(result.Contains("\"title\":\"Needlestick injury"));
        }

     

        // Care Advice Controller tests
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Fields()
        {
            var getQuestionEndpoint = "pathways";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsFalse(result.Contains("\"title\":\"Abdominal Pain"));
        }

    

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_ID()
        {
            var getQuestionEndpoint = "pathways/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"id\":\"P130"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsFalse(result.Contains("\"title\":\"Headache"));
            Assert.IsFalse(result.Contains("\"title\":\"Abdominal Pain"));
        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Male";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"id\":\"P130"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1401"));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=55&gender=Male";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_GenderChange()
        {
            var getQuestionEndpoint = "pathways/identify/{0}?age=0&gender=Female";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"id\":\"P131"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1401"));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathways/symptomGroup/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1110"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1110", ""));

        }
        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestGetNextQuestion()
        {
            var getNextQuestionEndpoint = "questions/{0}/answersNext";
            var expectedNextId = "PW756.300";
            var address = String.Format(_domainApiDomain + getNextQuestionEndpoint, _testQuestionId);
            
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
            var result = await _restfulHelper.GetAsync(String.Format(_domainApiDomain + getQuestionEndpoint, _expectedNextId));

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
