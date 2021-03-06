﻿using NHS111.Functional.Tests.Tools;
using NHS111.Models.Models.Business.Question;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using log4net;

namespace NHS111.Business.API.Functional.Tests
{
    [TestFixture]
    public class BusinessApiTests
    {
        private string _testPathwayNo2 = "PW752";
        private string _testPathwayNo3 = "PW628";
        private string _testPathwayNo = "PW1708";


        private ILoggingRestClient _restClient = new LoggingRestClient(ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], LogManager.GetLogger("log"));

        private static string BusinessApiPathwayUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessApiPathwayUrl"]; }
        }

        private static string BusinessApiPathwaySymptomGroupUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessApiPathwaySymptomGroupUrl"]; }
        }

        private static string BusinessApiNextNodeUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessApiNextNodeUrl"]; }
        }

        private static string BusinessApiFullJourneyUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessApiFullJourneyUrl"]; }
        }

        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "/Female/16";

            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2), Method.GET));
            var result = response.Content;
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
            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2), Method.GET));
            var result = response.Content;

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidAge200()
        {
            var getQuestionEndpoint = "/Female/200";
            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2), Method.GET));
            var result = response.Content;

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidAge15()
        {
            var getQuestionEndpoint = "/Female/15";
            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2), Method.GET));
            var result = response.Content;
            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }
        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Numbers_InvalidGender()
        {
            var getQuestionEndpoint = "/Male/16";
            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl + getQuestionEndpoint, _testPathwayNo2), Method.GET));
            var result = response.Content;

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));
        }

        [Test]
        public async void BusinessApiTests_returns_valid_Pathway_Symptom_Group()
        {
            var response = await _restClient.ExecuteAsync<string>(new JsonRestRequest(string.Format(BusinessApiPathwaySymptomGroupUrl, _testPathwayNo), Method.GET));
            var result = response.Data;
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
            var response = await _restClient.ExecuteAsync(new JsonRestRequest(string.Format(BusinessApiPathwayUrl, string.Empty), Method.GET));
            var result = response.Content;

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
            // Infant pathway so not in SearchData.csv and no [:isDescribedAs] relationship
            // so no longer returned
            // Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW754"));
        }

        [Test]
        //Test to show answer nodes are checked in the correct order so that 'evaluate variables' are handled correctly.
        public async void TestGetQuestion_returns_expected_Next_QuestionAgeVariable()
        {
            var expectedNexQuestionId = "PW628.13100";
            var NodeId = "PW628.10700";
            var state = "{\"PATIENT_AGE\":\"50\",\"PATIENT_GENDER\":\"\\\"F\\\"\",\"PATIENT_PARTY\":\"1\",\"PATIENT_AGEGROUP\":\"Adult\"}";
            var requestUrl = string.Format(BusinessApiNextNodeUrl, _testPathwayNo3, "Question", NodeId, HttpUtility.UrlEncode(state));

            var request = new JsonRestRequest(requestUrl, Method.POST);
            request.AddJsonBody("No");
            var response = await _restClient.ExecuteAsync(request);

            //this checks a responce is returned
            Assert.IsNotNull(response);

            var content = response.Content;

            //these check the right fields are returned
            Assert.IsTrue(content.Contains("\"id\":\"" + expectedNexQuestionId + "\""));
            SchemaValidation.AssertValidResponseSchema(content, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(content.Contains("\"questionNo\":\"TX220118"));
        }

        //Tests to show full journey returned given list of answered questions
        [TestCaseSource("FullJourneyTestCases")]
        public async void GetFullPathwayJourney_returns_expected_journey(IEnumerable<JourneyStep> journey, int totalJourneyLength, int totalQuestions, int totalInlineCareAdvices, int totalInterimCareAdvice, int totalReads, int totalSets, string startingpPathwayId, string dispositionCode, string traumaType, IDictionary<string, string> state)
        {
            var fullPathwayJourney = new FullPathwayJourney
            {
                JourneySteps = journey,
                StartingPathwayId = startingpPathwayId,
                StartingPathwayType = traumaType,
                DispostionCode = dispositionCode,
                State = state
            };
            var request = new JsonRestRequest(BusinessApiFullJourneyUrl, Method.POST);
            request.AddJsonBody(fullPathwayJourney);
            var result = await _restClient.ExecuteAsync<List<QuestionWithAnswers>>(request);

            //this checks a response is returned
            Assert.IsNotNull(result);

            var questions = result.Data;

            //check correct journey length and make up of nodes returned
            Assert.AreEqual(totalJourneyLength, questions.Count);
            Assert.AreEqual(totalQuestions, questions.Count(q => q.Labels.Contains("Question")));
            Assert.AreEqual(totalReads, questions.Count(q => q.Labels.Contains("Read")));
            Assert.AreEqual(totalSets, questions.Count(q => q.Labels.Contains("Set")));
            Assert.AreEqual(totalInterimCareAdvice, questions.Count(q => q.Labels.Contains("InterimCareAdvice")));
        }


        public IEnumerable<TestCaseData> FullJourneyTestCases
        {
            get
            {
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 19, 10, 0, 0, 2, 3, "PW1772FemaleChild", "Dx32", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "11" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Starts with question and no set/read nodes");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1719.0", Answer = new Answer { Order = 2 } },
                    new JourneyStep { QuestionId = "PW1719.1000", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1719.1300", Answer = new Answer { Order = 2 } }
                }, 22, 10, 0, 2, 3, 3, "PW1719MaleAdult", "Dx06", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "EndingLifeAsked", "\"present\"" } }).SetName("Starts with question ends with read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW711.100", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW998.10", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW998.800", Answer = new Answer { Order = 1 } }
                }, 25, 10, 0, 2, 5, 4, "PW711MaleAdult", "Dx0121", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "11" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "Fever", "\"present\"" }, { "SYSTEM_MERS", "mers" } }).SetName("Starts with set then read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1543.20", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW1543.3000", Answer = new Answer { Order = 2 } },
                    new JourneyStep { QuestionId = "PW1543.2600", Answer = new Answer { Order = 1 } }
                }, 21, 10, 0, 0, 4, 3, "PW1543MaleAdult", "Dx0124", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Starts with read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW711.100", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW998.10", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW998.800", Answer = new Answer { Order = 2 } }
                }, 25, 10, 0, 2, 5, 4, "PW711MaleAdult", "Dx0121", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "EndingLifeAsked", "\"present\"" }, { "SYSTEM_MERS", "mers" }, { "SYSTEM_COVIDLEVEL1", "covidlevel1" } }).SetName("Starts with set");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW975.10600", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW556.6800", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.100", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.300", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.14800", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.700", Answer = new Answer { Order = 1 } }
                }, 29, 13, 0, 2, 6, 4, "PW975FemaleAdult", "Dx0121", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" }, { "MERSSymptom", "present" } }).SetName("contains multiple reads");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1496.0", Answer = new Answer { Order = 4 } },
                    new JourneyStep { QuestionId = "PW1496.200", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW1034.0", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW1034.100", Answer = new Answer { Order = 1 } }

                }, 24, 11, 0, 2, 4, 3, "PW1496MaleAdult", "Dx012", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("contains multiple reads consecutively");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW975.10600", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW556.6800", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.100", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.300", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.14800", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW556.700", Answer = new Answer { Order = 1 } }
                }, 32, 16, 0, 2, 7, 3, "PW975FemaleAdult", "Dx0121", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" }, { "MERSSymptom", "present" } }).SetName("Female adult trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 22, 13, 0, 0, 3, 2, "PW1772FemaleChild", "Dx32", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "18" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Adult" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Male adult trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 25, 13, 0, 0, 5, 3, "PW1772FemaleChild", "Dx32", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "11" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Female child <12 trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 25, 13, 0, 0, 5, 3, "PW1772MaleChild", "Dx32", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "11" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Male child <12 trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 25, 13, 0, 0, 5, 3, "PW1772FemaleChild", "Dx32", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "14" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Female child >12 trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1772.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1621.1000", Answer = new Answer { Order = 1 } }
                }, 25, 13, 0, 0, 5, 3, "PW1772MaleChild", "Dx32", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "14" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" } }).SetName("Male child >12 trauma");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW564.0", Answer = new Answer { Order = 6 } },
                    new JourneyStep { QuestionId = "PW564.100", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW564.8700", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW564.8500", QuestionNo = "CX12345", Answer = new Answer { Order = 5 } },
                    new JourneyStep { QuestionId = "PW564.500", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW564.1500", Answer = new Answer { Order = 4 } },
                    new JourneyStep { QuestionId = "PW564.5600", Answer = new Answer { Order = 4 } },
                    new JourneyStep { QuestionId = "PW564.5500", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW564.6600", Answer = new Answer { Order = 2 } },
                    new JourneyStep { QuestionId = "PW564.7000", Answer = new Answer { Order = 3 } }
                }, 30, 19, 1, 0, 4, 2, "PW564MaleAdult", "Dx330", "Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "42" }, { "PATIENT_GENDER", "\"M\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" }, { "CX100037", "" } }).SetName("journey contains inline care advice");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PA215.32000", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PA215.9000", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PA215.9200", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PA215.26000", Answer = new Answer { Order = 2 } },
                    new JourneyStep { QuestionId = "PA215.28100", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PA215.36900", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PA215.9900", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PA215.10000", Answer = new Answer { Order = 2 } },
                    new JourneyStep { QuestionId = "PA215.10100", Answer = new Answer { Order = 4 } }
                }, 37, 16, 0, 0, 11, 6, "PW1746FemaleChild", "Dx34", "Non Trauma", new Dictionary<string, string> { { "PATIENT_AGE", "5" }, { "PATIENT_GENDER", "\"F\"" }, { "PATIENT_PARTY", "1" }, { "PATIENT_AGEGROUP", "Child" }, { "SYSTEM_ONLINE", "online" }, { "SYSTEM_MERS", "mers" }, { "NormalSugarReading", "\"present\"" }, { "ConcernHighBloodSugars", "\"present\"" }, { "MMOLperLitre", "\"present\"" } }).SetName("journey does not contain care advice");
            }
        }
    }
}
