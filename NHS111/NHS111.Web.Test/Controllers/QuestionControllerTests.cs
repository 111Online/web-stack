using NHS111.Utils.RestTools;
using NHS111.Web.Helpers;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Controllers
{
    using Features;
    using Logging;
    using Moq;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Controllers;
    using AwfulIdea = System.Tuple<string, NHS111.Models.Models.Web.QuestionViewModel>;
    using IConfiguration = Configuration.IConfiguration;

    [TestFixture]
    public class QuestionControllerTests
    {
        private string _pathwayId = "PW755MaleAdult";
        private int _age = 35;
        private string _pathwayTitle = "Headache";
        private Mock<IJourneyViewModelBuilder> _mockJourneyViewModelBuilder;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IDirectLinkingFeature> _mockFeature;
        private Mock<IJustToBeSafeFirstViewModelBuilder> _mockJtbsBuilderMock;
        private Mock<IAuditLogger> _mockAuditLogger;
        private Mock<IUserZoomDataBuilder> _mockUserZoomDataBuilder;
        private Mock<ILoggingRestClient> _mockRestClient;
        private Mock<IViewRouter> _mockViewRouter;
        private Mock<IDosEndpointFeature> _mockDosEndpointFeature;
        private Mock<IDOSSpecifyDispoTimeFeature> _mockDOSSpecifyDispoTimeFeature;
        private Mock<IOutcomeViewModelBuilder> _mockOutcomeViewModelBuilder;

        [SetUp]
        public void Setup()
        {
            _mockJourneyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockFeature = new Mock<IDirectLinkingFeature>();
            _mockFeature.Setup(c => c.IsEnabled).Returns(true);
            _mockJtbsBuilderMock = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _mockAuditLogger = new Mock<IAuditLogger>();
            _mockUserZoomDataBuilder = new Mock<IUserZoomDataBuilder>();
            _mockRestClient = new Mock<ILoggingRestClient>();
            _mockViewRouter = new Mock<IViewRouter>();
            _mockDosEndpointFeature = new Mock<IDosEndpointFeature>();
            _mockDOSSpecifyDispoTimeFeature = new Mock<IDOSSpecifyDispoTimeFeature>();
            _mockOutcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();

            _mockFeature.Setup(m => m.IsEnabled).Returns(true);
            _mockRestClient.Setup(r => r.ExecuteAsync<Pathway>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<Pathway>)new RestResponse<Pathway>() { ResponseStatus = ResponseStatus.Completed, Data = new Pathway { Gender = "Male" } }));

            _mockRestClient.Setup(r => r.ExecuteAsync<QuestionWithAnswers>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<QuestionWithAnswers>)new RestResponse<QuestionWithAnswers>() { ResponseStatus = ResponseStatus.Completed, Data = new QuestionWithAnswers() }));

            _mockConfiguration.Setup(c => c.IsPublic).Returns(false);
        }

        [Test]
        public void Direct_WithNoAnswers_ReturnsFirstQuestionOfPathway()
        {

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", new QuestionViewModel())));

            _mockViewRouter.Setup(v => v.Build(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => new QuestionResultViewModel(null));

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockDosEndpointFeature.Object, _mockDOSSpecifyDispoTimeFeature.Object, _mockOutcomeViewModelBuilder.Object);

            var result = sut.Direct(_pathwayId, _age, _pathwayTitle, "LS177NZ", null, true);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);
        }

        [Test]
        public async void Direct_WithAnswer_BuildsModelWithCorrectAnswer()
        {
            var mockQuestion = new QuestionViewModel
            {
                Answers = new List<Answer> {
                    new Answer {
                        Order = 2,
                        Title = "Second Answer"
                    },
                    new Answer {
                        Order = 1,
                        Title = "First Answer"
                    },
                }
            };

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", mockQuestion)));

            _mockRestClient.Setup(r => r.ExecuteAsync<QuestionWithAnswers>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<QuestionWithAnswers>)new RestResponse<QuestionWithAnswers>() { ResponseStatus = ResponseStatus.Completed, Data = new QuestionWithAnswers() { Answers = mockQuestion.Answers } }));

            _mockJourneyViewModelBuilder.Setup(j => j.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => StartedTask((JourneyViewModel)mockQuestion));

            _mockViewRouter.Setup(v => v.Build(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => new QuestionResultViewModel(null));

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockDosEndpointFeature.Object, _mockDOSSpecifyDispoTimeFeature.Object, _mockOutcomeViewModelBuilder.Object);

            var result = (ViewResult)await sut.Direct(_pathwayId, _age, _pathwayTitle, "LS177NZ", new[] { 0 }, true);
            var model = (QuestionViewModel)result.Model;

            Assert.IsTrue(model.SelectedAnswer.Contains(mockQuestion.Answers[1].Title));

        }

        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }

        [Test]
        public void Direct_WithAnswers_ProvidesAnswersToBuilder()
        {
            //var mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
            var mockQuestion = new QuestionViewModel
            {
                Answers = new List<Answer> {
                    new Answer {
                        Order = 1,
                        Title = "1"
                    },
                    new Answer {
                        Order = 2,
                        Title = "2"
                    },
                    new Answer {
                        Order = 3,
                        Title = "3"
                    }
                }
            };

            _mockJourneyViewModelBuilder.Setup(q => q.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => StartedTask((JourneyViewModel)mockQuestion));

            var sequence = new MockSequence();

            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "1"), It.IsAny<QuestionWithAnswers>()));
            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "2"), It.IsAny<QuestionWithAnswers>()));
            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "3"), It.IsAny<QuestionWithAnswers>()));

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", mockQuestion)));

            _mockViewRouter.Setup(v => v.Build(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => new QuestionResultViewModel(null));

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockDosEndpointFeature.Object, _mockDOSSpecifyDispoTimeFeature.Object, _mockOutcomeViewModelBuilder.Object);

            var pathwayId = "PW755MaleAdult";
            var age = 35;
            var pathwayTitle = "Headache";

            var result = sut.Direct(pathwayId, age, pathwayTitle, "LS177NZ", new[] { 0, 1, 2 }, true);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);

            _mockJourneyViewModelBuilder.VerifyAll();
        }

        [Test]
        public void Direct_WithDirectLinkingDisabled_ReturnsNotFoundResult()
        {
            _mockFeature.Setup(c => c.IsEnabled).Returns(false);

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockDosEndpointFeature.Object, _mockDOSSpecifyDispoTimeFeature.Object, _mockOutcomeViewModelBuilder.Object);

            var result = sut.Direct(null, 0, null, null, null, null);

            Assert.NotNull(result);
            Assert.IsInstanceOf<HttpNotFoundResult>(result.Result);
        }

    }
}