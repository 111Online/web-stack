using System.Linq;
using System.Web.Mvc;
using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Helpers
{
    [TestFixture]
    public class ViewDeterminerTests
    {
        private IViewRouter _viewDeterminer;

        [SetUp]
        public void Setup()
        {
            var view = new Mock<IView>();
            var engine = new Mock<IViewEngine>();
            var viewEngineResult = new ViewEngineResult(view.Object, engine.Object);
            engine.Setup(e => e.FindView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(viewEngineResult);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine.Object);

            _viewDeterminer = new ViewRouter(new Mock<IAuditLogger>().Object, new Mock<IUserZoomDataBuilder>().Object, new Mock<IJourneyViewModelEqualityComparer>().Object);
        }

        [Test]
        public void Null_JourneyResult_ReturnsQuestionView()
        {
            var result = _viewDeterminer.Build(null, null);
            Assert.IsInstanceOf<QuestionResultViewModel>(result);
            Assert.AreEqual(new QuestionResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void Default_JourneyResult_ReturnsQuestionView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.Pathway }, null);
            Assert.IsInstanceOf<QuestionResultViewModel>(result);
            Assert.AreEqual(new QuestionResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void Question_JourneyResult_ReturnsQuestionView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.Question }, null);
            Assert.IsInstanceOf<QuestionResultViewModel>(result);
            Assert.AreEqual(new QuestionResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void Page_JourneyResult_ReturnsPageView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.Page }, null);
            Assert.IsInstanceOf<PageResultViewModel>(result);
            Assert.AreEqual(new PageResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void CareAdvice_JourneyResult_ReturnsCareAdviceView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.CareAdvice }, null);
            Assert.IsInstanceOf<CareAdviceResultViewModel>(result);
            Assert.AreEqual(new CareAdviceResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void DeadEndJump_JourneyResult_ReturnsDeadEndJumpView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.DeadEndJump }, null);
            Assert.IsInstanceOf<DeadEndJumpResultViewModel>(result);
            Assert.AreEqual(new DeadEndJumpResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void PathwaySelectionJump_JourneyResult_ReturnsPathwaySelectionJumpView()
        {
            var result = _viewDeterminer.Build(new JourneyViewModel { NodeType = NodeType.PathwaySelectionJump }, null);
            Assert.IsInstanceOf<PathwaySelectionJumpResultViewModel>(result);
            Assert.AreEqual(new PathwaySelectionJumpResultViewModel(null).ViewName, result.ViewName);
        }

        [Test]
        public void Default_Outcome_JourneyResult_ReturnsOutcomeView()
        {
            var result = _viewDeterminer.Build(new OutcomeViewModel { NodeType = NodeType.Outcome, OutcomeGroup = OutcomeGroup.GP }, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/Primary_Care/" + OutcomeGroup.GP.Id, result.ViewName);
        }

        [Test]
        public void Callback999Journey_Outcome_JourneyResult_ReturnsCallback999JourneyOutcomeView()
        {
            var journeyModel = new OutcomeViewModel
            {
                NodeType = NodeType.Outcome,
                OutcomeGroup = OutcomeGroup.Call999Cat3,
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult
                {
                    Success = new SuccessObject<ServiceViewModel>
                    {
                        Services = new[] {new ServiceViewModel {OnlineDOSServiceType = OnlineDOSServiceType.Callback}}
                            .ToList()
                    }
                }
            };
            var result = _viewDeterminer.Build(journeyModel, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/Call_999_Callback", result.ViewName);
        }

        [Test]
        public void RecommendedService_Outcome_NoService_JourneyResult_ReturnsNoRecommendedServiceView()
        {
            var result = _viewDeterminer.Build(new OutcomeViewModel { NodeType = NodeType.Outcome, OutcomeGroup = OutcomeGroup.RepeatPrescription }, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/" + OutcomeGroup.RepeatPrescription.Id + "/RecommendedServiceNotOffered", result.ViewName);
        }

        [Test]
        public void RecommendedService_Outcome_JourneyResult_ReturnsRecommendedServiceView()
        {
            var result = _viewDeterminer.Build(new OutcomeViewModel { NodeType = NodeType.Outcome, OutcomeGroup = OutcomeGroup.RepeatPrescription, RecommendedService = new RecommendedServiceViewModel(), HasSeenPreamble = true }, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/" + OutcomeGroup.RepeatPrescription.Id + "/RecommendedService", result.ViewName);
        }

        [Test]
        public void RecommendedService_Outcome_Preamble_JourneyResult_ReturnsPreambleView()
        {
            var result = _viewDeterminer.Build(new OutcomeViewModel { NodeType = NodeType.Outcome, OutcomeGroup = OutcomeGroup.RepeatPrescription, RecommendedService = new RecommendedServiceViewModel() }, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/" + OutcomeGroup.RepeatPrescription.Id + "/Outcome_Preamble", result.ViewName);
        }

        [Test]
        public void RecommendedService_Outcome_GP_JourneyResult_ReturnsContactGPView()
        {
            var result = _viewDeterminer.Build(new OutcomeViewModel { NodeType = NodeType.Outcome, OutcomeGroup = OutcomeGroup.NoFurtherAction }, new Mock<ControllerContext>().Object);
            Assert.IsInstanceOf<OutcomeResultViewModel>(result);
            Assert.AreEqual("../Outcome/" + OutcomeGroup.NoFurtherAction.Id, result.ViewName);
        }
    }
}
