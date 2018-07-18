﻿

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NHS111.Functional.Tests.Tools;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NUnit.Framework;

namespace NHS111.Business.API.Functional.Tests
{
    [TestFixture]
    public class BusinessApiTests
    {
        private string _testQuestionId = "PW1346.1000";
        private string _testPathwayNo2 = "PW752";
        private string _testPathwayNo3 = "PW628";
        private string _testPathwayNo = "PW1708";
        private string _expectedNodeId = "PW752.200";
        private  string DxCode1 = "Dx12";
        private string _testQuestionId2 = "PW628.9800";
  

        private RestfulHelper _restfulHelper = new RestfulHelper();

        private static string BusinessApiPathwayUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiPathwayUrl"]); }
        }

        private static string BusinessApiPathwaySymptomGroupUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiPathwaySymptomGroupUrl"]); }
        }

        private static string BusinessApiNextNodeUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiNextNodeUrl"]); }
        }

        private static string BusinessApiFullJourneyUrl
        {
            get { return string.Format("{0}{1}", ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"], ConfigurationManager.AppSettings["BusinessApiFullJourneyUrl"]); }
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
            var state="{\"PATIENT_AGE\":\"50\",\"PATIENT_GENDER\":\"\\\"F\\\"\",\"PATIENT_PARTY\":\"1\",\"PATIENT_AGEGROUP\":\"Adult\"}";
            var requestUrl = string.Format(BusinessApiNextNodeUrl, _testPathwayNo3, "Question", NodeId,  HttpUtility.UrlEncode(state));
            var result = await _restfulHelper.PostAsync(requestUrl, RequestFormatting.CreateHTTPRequest("No"));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            var content = await result.Content.ReadAsStringAsync();

            //these check the right fields are returned
            Assert.IsTrue(content.Contains("\"id\":\"" + expectedNexQuestionId + "\""));
           SchemaValidation.AssertValidResponseSchema(content, SchemaValidation.ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(content.Contains("\"questionNo\":\"TX220118"));
        }

        //Tests to show full journey returned given list of answered questions
        [TestCaseSource(nameof(FullJourneyTestCases))]
        public async void GetFullPathwayJourney_returns_expected_journey(List<JourneyStep> journey, int totalJourneyLength, int totalQuestions, int totalReads, int totalSets)
        {
            var journeyJson = JsonConvert.SerializeObject(journey);
            var request = RequestFormatting.CreateHTTPRequest(journeyJson, string.Empty);
            var result = await _restfulHelper.PostAsync(BusinessApiFullJourneyUrl, request);

            //this checks a response is returned
            Assert.IsNotNull(result);

            var content = await result.Content.ReadAsStringAsync();
            var questions = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(content);

            //check correct journey length and make up of nodes returned
            Assert.AreEqual(totalJourneyLength, questions.Count);
            Assert.AreEqual(totalQuestions, questions.Count(q => q.Labels.Contains("Question")));
            Assert.AreEqual(totalReads, questions.Count(q => q.Labels.Contains("Read")));
            Assert.AreEqual(totalSets, questions.Count(q => q.Labels.Contains("Set")));
        }

        public IEnumerable<TestCaseData> FullJourneyTestCases
        {
            get
            {
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1618.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1618.300", Answer = new Answer { Order = 1 } }
                }, 2, 2, 0, 0).SetName("Starts with question and no set/read nodes");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1618.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1618.300", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1618.700", Answer = new Answer { Order = 1 } }
                }, 4, 3, 1, 0).SetName("Starts with question ends with read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW711.100", Answer = new Answer { Order = 1 } },
                    new JourneyStep { QuestionId = "PW998.0", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW998.800", Answer = new Answer { Order = 1 } }
                }, 5, 3, 1, 1).SetName("Starts with set then read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW1555.6", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1555.200", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW1555.100", Answer = new Answer { Order = 1 } }
                }, 4, 3, 1, 0).SetName("Starts with read");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW558.3400", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW558.100", Answer = new Answer { Order = 1 } }
                }, 3, 2, 0, 1).SetName("Starts with set");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW975.10600", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW975.100", Answer = new Answer { Order = 1 } }
                }, 3, 2, 0, 1).SetName("contains multiple sets");
                yield return new TestCaseData(new List<JourneyStep>
                {
                    new JourneyStep { QuestionId = "PW516.0", Answer = new Answer { Order = 7 } },
                    new JourneyStep { QuestionId = "PW516.500", Answer = new Answer { Order = 4 } },
                    new JourneyStep { QuestionId = "PW516.2200", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW516.1200", Answer = new Answer { Order = 4 } },
                    new JourneyStep { QuestionId = "PW516.33200", Answer = new Answer { Order = 3 } },
                    new JourneyStep { QuestionId = "PW516.2100", Answer = new Answer { Order = 2 } }
                }, 10, 6, 4, 0).SetName("contains multiple reads consecutively");
            }
        }
    }
}
