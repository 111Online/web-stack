using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Mappers.WebMappings;
using NUnit.Framework;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Models.Test.Mappers.WebMappings.JourneyViewModelMapper
{
    [TestFixture()]
    public class JourneyViewModelMapperTests
    {
        private string TEST_QUESTION_ID = "PW1751.200";
        private string TEST_QUESTION_TITLE = "Test question title";
        private string TEST_QUESTION_NO = "Tx1540";
        private string TEST_QUESTION_RATIONALE = "Some test rationale";

        [TestFixtureSetUp()]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.JourneyViewModelMapper>());
        }

        [Test()]
        public void FromQuestionToJourneyViewModelConverter_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }


        [Test()]
        public void FromQuestionToJourneyViewModelConverter_FromQuestionToJourneyViewModelConverter_nullJourney_Test()
        {

            var questionJson =
                BuildQuestionJson();

            var result = Mapper.Map<string, JourneyViewModel>(questionJson);
            AssertValidModel(result);
        }


        [Test()]
        public void FromQuestionToJourneyViewModelConverter_FromQuestionToJourneyViewModelConverter_Not_nullJourney_Test()
        {

            var existingJourney = new JourneyViewModel() {Id = TEST_QUESTION_ID, Title = TEST_QUESTION_TITLE};
            var questionJson = BuildQuestionJson();

            var result = Mapper.Map(questionJson, existingJourney);
            AssertValidModel(result);
        }

        [Test()]
        public void FromQuestionToJourneyViewModelConverter_FromQuestionWithAnswersToJourneyViewModelConverter_Test()
        {
            var questionsWithAnswers = new QuestionWithAnswers()
            {
                Answered = new Answer() { Title = "No", Order = 3},
                Answers = new List<Answer>() { new Answer() { Title = "test answer1", Order = 1 }
                    ,new Answer() { Title = "test answer2", Order = 2 }
                    ,new Answer() { Title = "test answer3", Order = 3 }},
                Question = new Question() { Title = TEST_QUESTION_TITLE, Id = TEST_QUESTION_ID, Rationale = TEST_QUESTION_RATIONALE, QuestionNo = TEST_QUESTION_NO},
                Labels = new List<string>() { "Question" }
            };

            var result =
                Mapper.Map<QuestionWithAnswers, JourneyViewModel>(
                    questionsWithAnswers);
            AssertValidModel(result);
        }

        private void AssertValidModel(JourneyViewModel result)
        {
            Assert.AreEqual(TEST_QUESTION_ID, result.Id);
            Assert.AreEqual(TEST_QUESTION_TITLE, result.Title);
            Assert.AreEqual(3, result.Answers.Count);
            Assert.AreEqual(TEST_QUESTION_NO, result.QuestionNo);
            Assert.AreEqual(TEST_QUESTION_RATIONALE, result.Rationale);
            Assert.AreEqual(NodeType.Question, result.NodeType);
        }

        private string BuildQuestionJson()
        {
            return @"{""Question"":{""group"":null,""order"":null,""topic"":null,""id"":""" + TEST_QUESTION_ID + @""",""questionNo"":""" + TEST_QUESTION_NO + @""",""title"":""" + TEST_QUESTION_TITLE + @""",""jtbs"":""PW1751-"",""jtbsText"":null,""rationale"":""" + TEST_QUESTION_RATIONALE + @"""},
                        ""Answers"":[
                            {""title"":""test answer1"",""titleWithoutSpaces"":""testanswer1"",""symptomDiscriminator"":"""",""supportingInfo"":""This means that you're behaving differently than normal."",""keywords"":"""",""order"":2},
                            {""title"":""test answer2"",""titleWithoutSpaces"":""testanswer2"",""symptomDiscriminator"":"""",""supportingInfo"":""This means you're hearing things from inside or outside your head. \nYou may have heard voices telling you to do things."",""keywords"":"""",""order"":1},
                            {""title"":""test answer3"",""titleWithoutSpaces"":""testanswer3"",""symptomDiscriminator"":"""",""supportingInfo"":"""",""keywords"":"""",""order"":3},
                            ],
                        ""Answered"":{""title"":""No"",""titleWithoutSpaces"":""No"",""symptomDiscriminator"":"""",""supportingInfo"":"""",""keywords"":"""",""order"":3},
                        ""Labels"":[""Question""],""State"":null}";
        }
    }
}
