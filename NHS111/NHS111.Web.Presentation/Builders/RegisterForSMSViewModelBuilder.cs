using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.RestTools;
using RestSharp;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class RegisterForSMSViewModelBuilder : IRegisterForSMSViewModelBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClientCaseDataCaptureApi;

        public RegisterForSMSViewModelBuilder(IConfiguration configuration, IRestClient restClientCaseDataCaptureApi)
        {
            _configuration = configuration;
            _restClientCaseDataCaptureApi = restClientCaseDataCaptureApi;
        }

        public async Task<SMSRegistrationViewModel> CaseDataCaptureApiGenerateVerificationCode(SendSmsOutcomeViewModel model)
        {
            var response = await SendRequestToDataCaptureApi<string>(_configuration.CaseDataCaptureApiGenerateVerificationCodeUrl, model.MobileNumber);

            var viewDeterminer = new SMSViewDeterminer(response.StatusCode, "Enter_Verification_Code_SMS");

            return BuildAndReturnResult<SMSViewDeterminer>(model, viewDeterminer);
        }

        public async Task<SMSRegistrationViewModel> CaseDataCaptureApiVerifyCode(SendSmsOutcomeViewModel model)
        {
            var requestData = Mapper.Map<VerifyCodeRequest>(model);

            var response = await SendRequestToDataCaptureApi<VerifyCodeRequest>(_configuration.CaseDataCaptureApiVerifyPhoneNumberUrl, requestData);

            var viewDeterminer = new SMSVerifyCodeViewDeterminer(response.StatusCode);

            return BuildAndReturnResult<SMSVerifyCodeViewDeterminer>(model, viewDeterminer);
        }

        public async Task<SMSRegistrationViewModel> CaseDataCaptureApiSubmitSMSRegistration(SendSmsOutcomeViewModel model)
        {
            var requestData = Mapper.Map<SubmitSMSRegistrationRequest>(model);

            var response = await SendRequestToDataCaptureApi<SubmitSMSRegistrationRequest>(_configuration.CaseDataCaptureApiSubmitSMSRegistrationMessageUrl, requestData);

            var viewDeterminer = new SMSViewDeterminer(response.StatusCode, "Confirmation_SMS");

            return BuildAndReturnResult<SMSViewDeterminer>(model, viewDeterminer);
        }

        private async Task<IRestResponse> SendRequestToDataCaptureApi<T>(string endPoint, T body)
        {
            var request = new JsonRestRequest(endPoint, Method.POST);

            request.AddJsonBody(body);

            return await _restClientCaseDataCaptureApi.ExecuteTaskAsync(request);
        }

        private SMSRegistrationViewModel BuildAndReturnResult<T>(SendSmsOutcomeViewModel model, T viewDeterminer) where T : SMSViewDeterminerBase
        {
            var smsRegisrationModel = new SMSRegistrationViewModel(model);

            viewDeterminer.Build(smsRegisrationModel);

            return smsRegisrationModel;
        }
    }

    public interface IRegisterForSMSViewModelBuilder
    {
        Task<SMSRegistrationViewModel> CaseDataCaptureApiGenerateVerificationCode(SendSmsOutcomeViewModel model);
        Task<SMSRegistrationViewModel> CaseDataCaptureApiVerifyCode(SendSmsOutcomeViewModel model);
        Task<SMSRegistrationViewModel> CaseDataCaptureApiSubmitSMSRegistration(SendSmsOutcomeViewModel model);
    }

    public abstract class SMSViewDeterminerBase
    {
        public HttpStatusCode StatusCode { get; set; }
        public string SuccessView { get; set; }

        public SMSViewDeterminerBase(HttpStatusCode statusCode, string successView = "")
        {
            SuccessView = successView;
            StatusCode = statusCode;
        }

        public void BaseBuild(SMSRegistrationViewModel model)
        {
            model.ViewName = SuccessView;

            if (StatusCode != HttpStatusCode.OK)
            {
                model.ViewName = "Failure_SMS";
            }

            if ((int)StatusCode == 429)
            {
                model.ViewName = "Too_Many_Requests_SMS";
            }
        }

        public virtual void Build(SMSRegistrationViewModel model)
        {
            BaseBuild(model);
        }
    }

    public class SMSViewDeterminer : SMSViewDeterminerBase
    {
        public SMSViewDeterminer(HttpStatusCode statusCode, string successView = "") 
            : base(statusCode, successView) { }
    }

    public class SMSVerifyCodeViewDeterminer : SMSViewDeterminerBase
    {
        public SMSVerifyCodeViewDeterminer(HttpStatusCode statusCode, string successView = "") 
            : base(statusCode, successView) { }

        public override void Build(SMSRegistrationViewModel model)
        {
            base.BaseBuild(model);
            if (StatusCode == HttpStatusCode.BadRequest)
            {
                model.ViewName = "Enter_Verification_Code_SMS";
                model.VerificationCodeIncorrect = true;
            }
        }
    }
}
