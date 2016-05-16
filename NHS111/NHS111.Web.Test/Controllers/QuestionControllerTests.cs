
namespace NHS111.Web.Presentation.Test.Controllers {
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Moq;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using Web.Controllers;

    [TestFixture]
    public class QuestionControllerTests {

        [Test]
        public void Direct_WithNoAnswers_ReturnsFirstQuestionOfPathway() {

            var mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
            mockQuestionViewModelBuilder.Setup(q => q.ActionSelection(It.IsAny<JourneyViewModel>()))
                .Returns(() => Task<Tuple<string, JourneyViewModel>>.Factory.StartNew(() => new Tuple<string, JourneyViewModel>("../Question/Question", new JourneyViewModel())));

            var sut = new QuestionController(mockQuestionViewModelBuilder.Object, null);

            var pathwayId = "PW755MaleAdult";
            var age = 35;
            var pathwayTitle = "Headache";

            var result = sut.Direct(pathwayId, age, pathwayTitle, null);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);
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