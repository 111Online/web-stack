using NHS111.Web.Helpers;
using RestSharp;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Controllers;
using NHS111.Web.Presentation.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System;

namespace NHS111.Web.Presentation.Test.Controllers
{

    [TestFixture]
    public class RegisterForSMSControllerTests
    {
        private Mock<IJourneyViewModelBuilder> _mockJourneyViewModelBuilder;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _mockRestClient;
        private Mock<IViewRouter> _mockViewRouter;
        private Mock<IRegisterForSMSViewModelBuilder> _mockRegisterForSmsViewModelBuilder;

        private Journey _journeyModel;
        private RegisterForSMSController _sut;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockRestClient = new Mock<IRestClient>();
            _mockViewRouter = new Mock<IViewRouter>();

            _mockJourneyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            _mockRegisterForSmsViewModelBuilder = new Mock<IRegisterForSMSViewModelBuilder>();

            _journeyModel = new Journey
            {
                Steps = new List<JourneyStep> {
                    new JourneyStep {
                        Answer = new Answer{
                            Title = "SomeTitle" }, QuestionNo = "DxC112", QuestionType = QuestionType.String, QuestionId = "", NodeType = NodeType.Question, AnswerInputValue= "123456"}
                }
            };

            _sut = new RegisterForSMSController(_mockRegisterForSmsViewModelBuilder.Object, _mockJourneyViewModelBuilder.Object, _mockConfiguration.Object, _mockRestClient.Object, _mockViewRouter.Object);
        }

        [Test]
        public void Calls_the_MessageCaseDataCaptureApi_with_the_correct_models_when_calling_SubmitSMSRegistration()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(AnyQuestionViewModel(), AnyQuestionWithAnswers())).ReturnsAsync(new JourneyViewModel { Journey = _journeyModel });
            var journeyJson = JsonConvert.SerializeObject(_journeyModel);
            
            var result = _sut.SubmitSMSRegistration(new SendSmsOutcomeViewModel
            {
                Journey = _journeyModel,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson
            });

            _mockRegisterForSmsViewModelBuilder.Verify(
                c => c.MessageCaseDataCaptureApi<SubmitSMSRegistrationRequest, SMSSubmitRegistrationViewDeterminer>(AnySendSmsOutcomeViewModel(), AnyString()), Times.Once
               );
        }

        [Test]
        public void Returns_the_expected_model_when_calling_SubmitSMSRegistration()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(AnyQuestionViewModel(), AnyQuestionWithAnswers())).ReturnsAsync(new JourneyViewModel { Journey = _journeyModel });
            var journeyJson = JsonConvert.SerializeObject(_journeyModel);
            var smsOutcomeViewModel = new SendSmsOutcomeViewModel
            {
                Journey = _journeyModel,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson,
            };
            var returnedModel = new SMSRegistrationViewModel(smsOutcomeViewModel);
            _mockRegisterForSmsViewModelBuilder.Setup(
                    s => s.MessageCaseDataCaptureApi<SubmitSMSRegistrationRequest, SMSSubmitRegistrationViewDeterminer>(AnySendSmsOutcomeViewModel(), AnyString()))
                .ReturnsAsync(returnedModel);

            var result = _sut.SubmitSMSRegistration(smsOutcomeViewModel);

            var model = (SendSmsOutcomeViewModel)((ViewResult)result.Result).Model;

            Assert.AreEqual("123456", model.Journey.Steps[0].AnswerInputValue);
        }

        [Test]
        public void Calls_the_MessageCaseDataCaptureApi_with_the_correct_models_when_calling_GetSMSSecurityCode()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(AnyQuestionViewModel(), AnyQuestionWithAnswers())).ReturnsAsync(new JourneyViewModel { Journey = _journeyModel });
            var journeyJson = JsonConvert.SerializeObject(_journeyModel);

            var result = _sut.GetSMSSecurityCode(new SendSmsOutcomeViewModel
            {
                Journey = _journeyModel,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson
            });

            _mockRegisterForSmsViewModelBuilder.Verify(
                c => c.MessageCaseDataCaptureApi<GenerateSMSVerifyCodeRequest, SMSGenerateCodeViewDeterminer>(AnySendSmsOutcomeViewModel(), AnyString()), Times.Once
               );
        }

        [Test]
        public void Returns_the_expected_model_when_calling_GetSMSSecurityCode()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(AnyQuestionViewModel(), AnyQuestionWithAnswers())).ReturnsAsync(new JourneyViewModel { Journey = _journeyModel });
            var journeyJson = JsonConvert.SerializeObject(_journeyModel);
            var smsOutcomeViewModel = new SendSmsOutcomeViewModel
            {
                Journey = _journeyModel,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson,
            };
            var returnedModel = new SMSRegistrationViewModel(smsOutcomeViewModel);
            _mockRegisterForSmsViewModelBuilder.Setup(
                    s => s.MessageCaseDataCaptureApi<GenerateSMSVerifyCodeRequest, SMSGenerateCodeViewDeterminer>(AnySendSmsOutcomeViewModel(), AnyString()))
                .ReturnsAsync(returnedModel);

            var result = _sut.GetSMSSecurityCode(smsOutcomeViewModel);

            var model = (SendSmsOutcomeViewModel)((ViewResult)result.Result).Model;

            Assert.AreEqual("123456", model.Journey.Steps[0].AnswerInputValue);
        }

        [Test]
        public void Returns_the_expected_view_when_calling_SubmitSMSSecurityCode_with_invalid_model()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(AnyQuestionViewModel(), AnyQuestionWithAnswers())).ReturnsAsync(new JourneyViewModel { Journey = _journeyModel });
            _sut.ModelState.AddModelError("myError", "myErrroMessage");
            var journeyJson = JsonConvert.SerializeObject(_journeyModel);

            var smsOutcomeViewModel = new SendSmsOutcomeViewModel
            {
                Journey = _journeyModel,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "" },
                JourneyJson = journeyJson,
            };

            var result = _sut.SubmitSMSSecurityCode(smsOutcomeViewModel);

            var viewResult = ((ViewResult)result.Result);

            Assert.AreEqual("Enter_Verification_Code_SMS", viewResult.ViewName);

        }

        private static QuestionWithAnswers AnyQuestionWithAnswers()
        {
            return It.IsAny<QuestionWithAnswers>();
        }

        private static SendSmsOutcomeViewModel AnySendSmsOutcomeViewModel()
        {
            return It.IsAny<SendSmsOutcomeViewModel>();
        }

        private static string AnyString()
        {
            return It.IsAny<string>();
        }
        private static QuestionViewModel AnyQuestionViewModel()
        {
            return It.IsAny<QuestionViewModel>();
        }
    }
}