
namespace NHS111.Web.Presentation.Test.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Configuration;
    using Moq;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using Utils.Helpers;
    using Web.Controllers;

    [TestFixture]
    public class QuestionControllerTests {
        private string _pathwayId = "PW755MaleAdult";
        private int _age = 35;
        private string _pathwayTitle = "Headache";
        private Mock<IJourneyViewModelBuilder> _mockJourneyViewModelBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<IConfiguration> _mockConfiguration;

        [TestFixtureSetUp]
        public void Setup() {
            _mockJourneyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<IConfiguration>();
        }

        [Test]
        public void Direct_WithNoAnswers_ReturnsFirstQuestionOfPathway() {
            _mockJourneyViewModelBuilder.Setup(
                q => q.Build(It.IsAny<JourneyViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => Task<JourneyViewModel>.Factory.StartNew(() => new JourneyViewModel()));

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object, _mockRestfulHelper.Object,
                _mockConfiguration.Object);

            var result = sut.Direct(_pathwayId, _age, _pathwayTitle, null);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);
        }

        [Test]
        public async void Direct_WithAnswer_BuildsModelWithCorrectAnswer() {
            var mockJourney = new JourneyViewModel {
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
            var mockJourneyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            mockJourneyViewModelBuilder.Setup(
                q => q.Build(It.IsAny<JourneyViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => Task<JourneyViewModel>.Factory.StartNew(() => mockJourney));

            var sut = new QuestionController(mockJourneyViewModelBuilder.Object, _mockRestfulHelper.Object,
                _mockConfiguration.Object);
            var result = (ViewResult) await sut.Direct(_pathwayId, _age, _pathwayTitle, new[] {0});
            var model = (JourneyViewModel) result.Model;

            Assert.IsTrue(model.SelectedAnswer.Contains(mockJourney.Answers[1].Title));

        }

        //[Test]
        //public void Direct_WithAnswers_ProvidesAnswersToBuilder() {

        //    var mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
        //    mockQuestionViewModelBuilder.Setup(q => q.ActionSelection(It.IsAny<JourneyViewModel>()))
        //        .Returns(() => Task<Tuple<string, JourneyViewModel>>.Factory.StartNew(
        //                    () => new Tuple<string, JourneyViewModel>("../Question/Question", new JourneyViewModel())));

        //    var sequence = new MockSequence();

        //    mockQuestionViewModelBuilder.InSequence(sequence)
        //        .Setup(x => x.BuildQuestion(It.Is<JourneyViewModel>(j => j.SelectedAnswer == "1")));
        //    mockQuestionViewModelBuilder.InSequence(sequence)
        //        .Setup(x => x.BuildQuestion(It.Is<JourneyViewModel>(j => j.SelectedAnswer == "2")));
        //    mockQuestionViewModelBuilder.InSequence(sequence)
        //        .Setup(x => x.BuildQuestion(It.Is<JourneyViewModel>(j => j.SelectedAnswer == "3")));

        //    var sut = new QuestionController(mockQuestionViewModelBuilder.Object, null);

        //    var pathwayId = "PW755MaleAdult";
        //    var age = 35;
        //    var pathwayTitle = "Headache";

        //    var result = sut.Direct(pathwayId, age, pathwayTitle, new [] {1, 2, 3});

        //    Assert.IsInstanceOf<ViewResult>(result.Result);
        //    var viewResult = result.Result as ViewResult;
        //    Assert.AreEqual("../Question/Question", viewResult.ViewName);

        //    mockQuestionViewModelBuilder.VerifyAll();
        //}
    }
}