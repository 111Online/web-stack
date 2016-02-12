
namespace NHS111.Business.API.Functional.Tests
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Text;
    using Utils.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ProductionQuestionEndpointTests
    {
        private string _BusinessdomainProductionApiDomain =
            "https://microsoft-apiapp40f6723d48db47ed8f4d3ff12ebe22c1.azurewebsites.net/";

        private string _testQuestionId = "PW1346.1000";
        private string _testPathwayNo2 = "PW752";
        private string _testPathwayNo = "PW1401";
        private string _expectedNodeId = "PW752.200";


        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>


        // Care Advice Controller tests
        [Test]
        public async void TestGetQuestion_returns_valid_care_advice_AdultAge()
        {
            var getQuestionEndpoint = "pathways/care-advice/43/Female?markers=Cx220179";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint));

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
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint));

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


        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo2));

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
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge200()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/200";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge15()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/15";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidGender()
        {
            var getQuestionEndpoint = "pathway/{0}/Male/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo2));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathway/symptomGroup/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1110"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1110", ""));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway()
        {
            var getQuestionEndpoint = "pathway";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"), "No headache title");
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"), "No head, Facial or Neck Injury, Blunt title ");
            Assert.IsTrue(result.Contains("\"id\":\"P130"), "No P130 id");
            Assert.IsTrue(result.Contains("\"id\":\"P275"), "No P270 id");
            Assert.IsTrue(result.Contains("\"gender\":\"Female"), "No gender Female");
            Assert.IsTrue(result.Contains("\"gender\":\"Male"), "No gender Male");
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW684"), "No pathway PW684");
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1401"), "No pathway PW1401");

        }

        [Test]
        //pathway_suggest/{name}
        public async void TestGetQuestion_returns_expected_Pathways_beginning_with()
        {
            var getQuestionEndpoint = "pathway_suggest/Head";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainProductionApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"pathwayNumbers\":[\"PW753\",\"PW756\",\"PW752\",\"PW755\",\"PW754\",\"PW754\"],\"group\":\"Headache\"}"));
            Assert.IsTrue(result.Contains("\"pathwayNumbers\":[\"PW1401\",\"PW1401\",\"PW686\",\"PW686\",\"PW684\",\"PW684\",\"PW684\",\"PW684\"],\"group\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"pathwayNumbers\":[\"PW692\",\"PW692\",\"PW692\",\"PW692\",\"PW1403\",\"PW1403\",\"PW694\",\"PW694\"],\"group\":\"Head, Facial or Neck Injury, Penetrating"));
            Assert.IsTrue(result.Contains("\"pathwayNumbers\":[\"PW1679\",\"PW1679\",\"PW1679\",\"PW1679\",\"PW1679\",\"PW1679\"],\"group\":\"Head Lice (Declared)"));
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
