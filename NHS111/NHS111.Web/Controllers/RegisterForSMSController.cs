using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Utils.RestTools;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    public class RegisterForSMSController : Controller
    {
        private readonly IRegisterForSMSViewModelBuilder _registerForSmsViewModelBuilder;
        private readonly IQuestionNavigiationService _questionNavigiationService;
        private readonly IConfiguration _configuration;
        private readonly IViewRouter _viewRouter;

        public RegisterForSMSController(IRegisterForSMSViewModelBuilder registerForSmsViewModelBuilder,
            IJourneyViewModelBuilder journeyViewModelBuilder, IConfiguration configuration, ILoggingRestClient restClientBusinessApi, IViewRouter viewRouter)
        {
            _registerForSmsViewModelBuilder = registerForSmsViewModelBuilder;
            _configuration = configuration;
            _viewRouter = viewRouter;
            _questionNavigiationService = new QuestionNavigationService(journeyViewModelBuilder, configuration,
                restClientBusinessApi, viewRouter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetSMSSecurityCode(SendSmsOutcomeViewModel model)
        {
            var result = await _registerForSmsViewModelBuilder
                .MessageCaseDataCaptureApi<GenerateSMSVerifyCodeRequest, SMSGenerateCodeViewDeterminer>(model, _configuration.CaseDataCaptureApiGenerateVerificationCodeUrl);

            ModelState.Clear();

            return View(result.ViewName, result.SendSmsOutcomeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitSMSSecurityCode(SendSmsOutcomeViewModel model)
        {
            if (VerificationCodeInputIsNotValid())
                return View("Enter_Verification_Code_SMS", model);

            var result = await _registerForSmsViewModelBuilder
                .MessageCaseDataCaptureApi<VerifySMSCodeRequest, SMSEnterVerificationCodeViewDeterminer>(model, _configuration.CaseDataCaptureApiVerifyPhoneNumberUrl);

            return string.IsNullOrWhiteSpace(result.ViewName)
                ? await RedirectToNextQuestion(model) : View(result.ViewName, result.SendSmsOutcomeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitSMSRegistration(SendSmsOutcomeViewModel model)
        {
            model.VerificationCodeInput = GetVerificationCodeInputFromJourney(model);

            var result = await _registerForSmsViewModelBuilder
                .MessageCaseDataCaptureApi<SubmitSMSRegistrationRequest, SMSSubmitRegistrationViewDeterminer>(model, _configuration.CaseDataCaptureApiSubmitSMSRegistrationMessageUrl);

            return View(result.ViewName, result.SendSmsOutcomeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GoBackToSMSVerifyStart(SendSmsOutcomeViewModel model)
        {
            //var VerifyForSMSViewModel = new VerifyForSMSViewModel(model);
            var verifyForSMSViewModel = _viewRouter.Build(model, ControllerContext);
            return View(verifyForSMSViewModel.ViewName, model);
        }

        private bool VerificationCodeInputIsNotValid()
        {
            return ModelState["VerificationCodeInput"] != null && ModelState["VerificationCodeInput"].Errors.Count > 0;
        }

        private async Task<ActionResult> RedirectToNextQuestion(SendSmsOutcomeViewModel model)
        {
            ModelState.Clear();

            var questionViewModel = CovertSendSmsOutcomeViewModelToQuestionViewModel(model);

            var viewRouter = await _questionNavigiationService.NextQuestion(questionViewModel, ControllerContext);

            return View(viewRouter.ViewName, viewRouter.JourneyModel);
        }

        private static QuestionViewModel CovertSendSmsOutcomeViewModelToQuestionViewModel(SendSmsOutcomeViewModel model)
        {
            var questionViewModel = Mapper.Map<QuestionViewModel>(model);
            questionViewModel.Journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            questionViewModel.Journey.Steps.Add(new JourneyStep()
            {
                Answer = new Answer(),
                AnswerInputValue = model.VerificationCodeInput.InputValue,
                QuestionId = "DxC112",
                QuestionNo = "DxC112",
                QuestionType = QuestionType.String
            });
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            answer.Title = "verify";
            questionViewModel.SelectedAnswer = JsonConvert.SerializeObject(answer);
            questionViewModel.AnswerInputValue = model.MobileNumber;
            return questionViewModel;
        }

        private VerificationCodeInputViewModel GetVerificationCodeInputFromJourney(SendSmsOutcomeViewModel model)
        {
            var verificationCodeInput = JsonConvert.DeserializeObject<Journey>(model.JourneyJson)
                .GetStepInputValue<string>(QuestionType.String, "DxC112");
            return new VerificationCodeInputViewModel() { InputValue = verificationCodeInput };
        }
    }
}