
namespace NHS111.Web.Presentation.Test.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Moq;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using Web.Controllers;

    [TestFixture]
    public class QuestionControllerTests {
        private string _pathwayId = "PW755MaleAdult";
        private int _age = 35;
        private string _pathwayTitle = "Headache";
        private Mock<IQuestionViewModelBuilder> _mockQuestionViewModelBuilder;

        [TestFixtureSetUp]
        public void Setup() {
            _mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
        }

        [Test]
        public void Direct_WithNoAnswers_ReturnsFirstQuestionOfPathway() {
            _mockQuestionViewModelBuilder.Setup(q => q.ActionSelection(It.IsAny<JourneyViewModel>()))
                .Returns(() => Task<Tuple<string, JourneyViewModel>>.Factory.StartNew(() => new Tuple<string, JourneyViewModel>("../Question/Question", new JourneyViewModel())));

            var sut = new QuestionController(_mockQuestionViewModelBuilder.Object, null);

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
            var mockTaskOfTuple = Task<Tuple<string, JourneyViewModel>>.Factory.StartNew(() => new Tuple<string, JourneyViewModel>("../Question/Question", mockJourney));
            var mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
            mockQuestionViewModelBuilder.Setup(q => q.ActionSelection(It.IsAny<JourneyViewModel>()))
                .Returns(() => mockTaskOfTuple);
            mockQuestionViewModelBuilder.Setup(q => q.BuildQuestion(It.IsAny<JourneyViewModel>()))
                .Returns(() => mockTaskOfTuple);

            var sut = new QuestionController(mockQuestionViewModelBuilder.Object, null);
            var result = (ViewResult) await sut.Direct(_pathwayId, _age, _pathwayTitle, new [] { 0 });
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