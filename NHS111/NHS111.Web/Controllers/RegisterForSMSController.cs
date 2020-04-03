using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using RestSharp;
using StructureMap.Query;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    public class RegisterForSMSController : Controller
    {
        private readonly IRegisterForSMSViewModelBuilder _registerForSmsViewModelBuilder;
        private readonly IQuestionNavigiationService _questionNavigiationService;

        public RegisterForSMSController(IRegisterForSMSViewModelBuilder registerForSmsViewModelBuilder, 
            IJourneyViewModelBuilder journeyViewModelBuilder, IConfiguration configuration, IRestClient restClientBusinessApi, IViewRouter viewRouter)
        {
            _registerForSmsViewModelBuilder = registerForSmsViewModelBuilder;
            _questionNavigiationService = new QuestionNavigationService(journeyViewModelBuilder, configuration,
                restClientBusinessApi, viewRouter);
        }

        [HttpPost]
        public async Task<ActionResult> GetSMSSecurityCode(SendSmsOutcomeViewModel model)
        {
            var smsRegistrationViewModel = await
                _registerForSmsViewModelBuilder.CaseDataCaptureApiGenerateVerificationCode(model);

            ModelState.Clear();

            return View(smsRegistrationViewModel.ViewName, smsRegistrationViewModel.SendSmsOutcomeViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitSMSSecurityCode(SendSmsOutcomeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Enter_Security_Code", model);

            var result = await _registerForSmsViewModelBuilder.CaseDataCaptureApiVerifyCode(model);

            return string.IsNullOrWhiteSpace(result.ViewName)
                ? await RedirectToNextQuestion(model) : View(result.ViewName, result.SendSmsOutcomeViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitSMSRegistration(SendSmsOutcomeViewModel model)
        {
            //TODO: refactor result model buider for itk and data cap +
            // must inheirt from JourneyViewModel to be logged
            var smsRegistrationViewModel = await _registerForSmsViewModelBuilder.CaseDataCaptureApiSubmitSMSRegistration(model);

            return View(smsRegistrationViewModel.ViewName, model);
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
                AnswerInputValue = model.VerificationCodeInput,
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
    }
}