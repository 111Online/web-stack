using System.Collections.Generic;
using FakeItEasy;
using Newtonsoft.Json;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class SurveyLinkViewModelBuilderTests
    {
        private SurveyLinkViewModelBuilder _sut;
        private OutcomeViewModel _outcomeViewModel;

        [SetUp]
        public void Setup()
        {
            var _fakeConfiguration = A.Fake<IConfiguration>();
            var _fakeLoggingRestClient = A.Fake<ILoggingRestClient>();
            var _fakeSurveyLinkFeature = A.Fake<ISurveyLinkFeature>();

            var _fakeRestResponse = A.Fake<IRestResponse<Pathway>>();

            var journeyModel = new Journey
            {
                Steps = new List<JourneyStep> {
                    new JourneyStep {
                        Answer = new Answer{
                            Title = "SomeTitle" }, QuestionNo = "DxC112", QuestionType = QuestionType.String, QuestionId = "", NodeType = NodeType.Question, AnswerInputValue= "123456"}
                }
            };
            _outcomeViewModel = new OutcomeViewModel
            {
                Id = "TestId",
                OutcomeGroup = OutcomeGroup.ItkPrimaryCare,
                JourneyJson = JsonConvert.SerializeObject(journeyModel),
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Gender = "Male", Age = 30 } }
            };

            A.CallTo(() => _fakeRestResponse.IsSuccessful).Returns(true);
            A.CallTo(() => _fakeRestResponse.Data).Returns(new Pathway());
            A.CallTo(() => _fakeLoggingRestClient.ExecuteAsync<Pathway>(A<JsonRestRequest>.Ignored)).Returns(_fakeRestResponse);
            A.CallTo(() => _fakeSurveyLinkFeature.PharmacySurveyId).Returns(string.Empty);
            A.CallTo(() => _fakeSurveyLinkFeature.SurveyId).Returns(string.Empty);

            _sut = new SurveyLinkViewModelBuilder(_fakeLoggingRestClient, _fakeConfiguration, _fakeSurveyLinkFeature);
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_visited_guided_selection_and_user_selects_none_of_these()
        {
            _outcomeViewModel.ViaGuidedSelection = false;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("false", result.GuidedSelection);
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_visited_guided_selection_and_user_selects_one_symptom()
        {
            _outcomeViewModel.ViaGuidedSelection = true;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual("true", result.GuidedSelection);
        }

        [Test]
        public void GuidedSelection_has_correct_value_when_user_has_not_visited_guid4ed_selection()
        {
            _outcomeViewModel.ViaGuidedSelection = null;
            var result = _sut.SurveyLinkBuilder(_outcomeViewModel).Result;
            Assert.AreEqual(string.Empty, result.GuidedSelection);
        }
    }
}
