
using System;
using NHS111.Web.Helpers;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Features;
    using Logging;
    using Moq;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using Utils.Helpers;
    using Web.Controllers;
    using IConfiguration = Configuration.IConfiguration;
    using AwfulIdea = System.Tuple<string, NHS111.Models.Models.Web.QuestionViewModel>;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NHS111.Models.Models.Web.Enums;
    using NHS111.Models.Models.Web.DataCapture;

    [TestFixture]
    public class RegisterForSMSControllerTests
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
        private Mock<IRestClient> _mockRestClient;
        private Mock<IViewRouter> _mockViewRouter;
        private Mock<IDosEndpointFeature> _mockDosEndpointFeature;
        private Mock<IDOSSpecifyDispoTimeFeature> _mockDOSSpecifyDispoTimeFeature;
        private Mock<IOutcomeViewModelBuilder> _mockOutcomeViewModelBuilder;
        private Mock<IRegisterForSMSViewModelBuilder> _mockRegisterForSmsViewModelBuilder;
        private JourneyViewModel _journeyViewModel;
        private RegisterForSMSController _sut;

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
            _mockRestClient = new Mock<IRestClient>();
            _mockViewRouter = new Mock<IViewRouter>();
            _mockDosEndpointFeature = new Mock<IDosEndpointFeature>();
            _mockDOSSpecifyDispoTimeFeature = new Mock<IDOSSpecifyDispoTimeFeature>();
            _mockOutcomeViewModelBuilder = new Mock<IOutcomeViewModelBuilder>();
            _mockRegisterForSmsViewModelBuilder = new Mock<IRegisterForSMSViewModelBuilder>();

            _mockRestClient.Setup(r => r.ExecuteTaskAsync<Pathway>(It.IsAny<RestRequest>())).Returns(
                () => StartedTask((IRestResponse<Pathway>)new RestResponse<Pathway>() 
                        { ResponseStatus = ResponseStatus.Completed, Data = new Pathway { Gender = "Male" } })
                );
            _mockRestClient.Setup(r => r.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<RestRequest>())).Returns(
                () => StartedTask((IRestResponse<QuestionWithAnswers>)new RestResponse<QuestionWithAnswers>()
                { ResponseStatus = ResponseStatus.Completed, Data = new QuestionWithAnswers() })
                );

            _mockConfiguration.Setup(c => c.IsPublic).Returns(false);

            _mockViewRouter.Setup(v => v.Build(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => new QuestionResultViewModel(null));

            _journeyViewModel = new JourneyViewModel
            {
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel { Age = 77, Gender = "Male" }
                },
                Journey = new Journey
                {
                    Steps = new List<JourneyStep> {
                        new JourneyStep {
                            Answer = new Answer{
                                Title = "SomeTitle" }, QuestionNo = "DxC112", QuestionType = QuestionType.String, QuestionId = "", NodeType = NodeType.Question, AnswerInputValue= "123456"}
                    }
                }
            };

            _sut = new RegisterForSMSController(_mockRegisterForSmsViewModelBuilder.Object, _mockJourneyViewModelBuilder.Object, _mockConfiguration.Object, _mockRestClient.Object, _mockViewRouter.Object);
        }

        [Test]
        public void With_a_somewhat_valid_view_model_calls_the_MessageCaseDataCaptureApi()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>())).ReturnsAsync(_journeyViewModel);
            var journeyJson = JsonConvert.SerializeObject(_journeyViewModel);

            var result = _sut.SubmitSMSRegistration(new SendSmsOutcomeViewModel
            {
                Journey = _journeyViewModel.Journey,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson
            });

            _mockRegisterForSmsViewModelBuilder.Verify(
                c => c.MessageCaseDataCaptureApi<SubmitSMSRegistrationRequest, SMSSubmitRegistrationViewDeterminer>(It.IsAny<SendSmsOutcomeViewModel>(), It.IsAny<string>()), Times.Once
               );
        }

        [Test]
        public void With_a_somewhat_valid_view_model_returns_the_expected_result()
        {
            _mockJourneyViewModelBuilder.Setup(r => r.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>())).ReturnsAsync(_journeyViewModel);

            _mockRegisterForSmsViewModelBuilder.Setup(s => s.MessageCaseDataCaptureApi<It.IsAny<object>, It.IsAny<object>>(It.IsAny<object>, It.IsAny<string>)

            //_mockRestClient.Setup(r => r.ExecuteTaskAsync<SendSmsOutcomeViewModel>(It.IsAny<RestRequest>())).Returns(
            //    () => StartedTask((IRestResponse<SMSRegistrationViewModel>)new RestResponse<SMSRegistrationViewModel>()
            //    { ResponseStatus = ResponseStatus.Completed, Data = new SMSRegistrationViewModel(It.IsAny<SendSmsOutcomeViewModel>) }
            //    )
            //    );

            var journeyJson = JsonConvert.SerializeObject(_journeyViewModel);

            var result = _sut.SubmitSMSRegistration(new SendSmsOutcomeViewModel
            {
                Journey = _journeyViewModel.Journey,
                VerificationCodeInput = new VerificationCodeInputViewModel { InputValue = "DxC112" },
                JourneyJson = journeyJson
            }
            );
        }


        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }
    }
}