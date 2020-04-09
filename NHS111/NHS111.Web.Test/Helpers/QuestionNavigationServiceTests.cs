using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NHS111.Web.Presentation.Test.Helpers
{
    [TestFixture]
    public class QuestionNavigationServiceTests
    {
        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestClient> _restClient;
        private Mock<IViewRouter> _viewRouter;
        private Mock<IJourneyViewModelBuilder> _journeyViewModelBuilder;
        private QuestionNavigationService _sut;

        private static Answer _answer = new Answer { Title = "Answer1" };
        private QuestionViewModel _questionViewModel = new QuestionViewModel { 
                PathwayId = "pathwayId", NodeType = NodeType.PathwaySelectionJump, StateJson = "", 
                SelectedAnswer = "{  \"title\": \"Answer1\",  \"titleWithoutSpaces\": \"\",  \"symptomDiscriminator\": null,  \"supportingInformation\": null,  \"keywords\": null,  \"excludeKeywords\": null,  \"reportText\": null,  \"dispositionDisplayText\": null,  \"order\": 0,  \"isPositive\": false,  \"specifyText\": null}",
                Answers = new List<Answer> { _answer}
                };

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _configuration.Setup(x => x.GetBusinessApiNextNodeUrl(It.IsAny<string>(), It.IsAny<NodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns("http://test.com/node/{0}/{1}/next_node/{2}?state={3}");

            _journeyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            _journeyViewModelBuilder
                .Setup(x => x.Build(
                    It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .ReturnsAsync(new JourneyViewModel { });

            var response = new Mock<IRestResponse<QuestionWithAnswers>>();
            response.Setup(_ => _.Data).Returns(new QuestionWithAnswers { });
            _restClient = new Mock<IRestClient>();
            _restClient.Setup(x => x.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<IRestRequest>())).ReturnsAsync(response.Object);

            _viewRouter = new Mock<IViewRouter>();
            _viewRouter.Setup(x => x.Build(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(new QuestionResultViewModel(null) { });

            _sut = new QuestionNavigationService(_journeyViewModelBuilder.Object, _configuration.Object, _restClient.Object, _viewRouter.Object);
        }

        [Test]
        public void GetNextNode_returns_a_question_with_answers_model()
        {
            var result = _sut.GetNextNode(_questionViewModel).Result;

            Assert.IsInstanceOf<QuestionWithAnswers>(result);
        }

        [Test]
        public void GetNextJourneyViewModel_returns_a_journey_view_model()
        {
            var result = _sut.GetNextJourneyViewModel(_questionViewModel).Result;

            Assert.IsInstanceOf<JourneyViewModel>(result);
        }

        [Test]
        public void GetNextQuestion_returns_a_journey_result_view_model()
        {
            var result = _sut.NextQuestion(_questionViewModel, null).Result;

            Assert.IsInstanceOf<JourneyResultViewModel>(result);
        }
    }
}
