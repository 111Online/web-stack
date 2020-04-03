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

            return BuildAndReturnResult(model, response, "Enter_Security_Code");
        }

        public async Task<SMSRegistrationViewModel> CaseDataCaptureApiVerifyCode(SendSmsOutcomeViewModel model)
        {
            var requestData = Mapper.Map<VerifyCodeRequest>(model);

            var response = await SendRequestToDataCaptureApi<VerifyCodeRequest>(_configuration.CaseDataCaptureApiVerifyPhoneNumberUrl, requestData);

            return BuildAndReturnResult(model, response);
        }

        public async Task<SMSRegistrationViewModel> CaseDataCaptureApiSubmitSMSRegistration(SendSmsOutcomeViewModel model)
        {
            var requestData = Mapper.Map<SubmitSMSRegistrationRequest>(model);

            var response = await SendRequestToDataCaptureApi<SubmitSMSRegistrationRequest>(_configuration.CaseDataCaptureApiSubmitSMSRegistrationMessageUrl, requestData);

            return BuildAndReturnResult(model, response, "Confirmation_SMS");
        }

        private async Task<IRestResponse> SendRequestToDataCaptureApi<T>(string endPoint, T body)
        {
            var request = new JsonRestRequest(endPoint, Method.POST);

            request.AddJsonBody(body);

            return await _restClientCaseDataCaptureApi.ExecuteTaskAsync(request);
        }

        private SMSRegistrationViewModel BuildAndReturnResult(SendSmsOutcomeViewModel model, IRestResponse response, string successView = "")
        {
            var smsRegisrationModel = new SMSRegistrationViewModel(model);

            smsRegisrationModel.ViewName = successView;

            Build(response.StatusCode, smsRegisrationModel);

            return smsRegisrationModel;
        }

        private void Build(HttpStatusCode statusCode, SMSRegistrationViewModel model)
        {
            if ((int)statusCode == 429)
            {
                model.ViewName = "Too_Many_Requests_SMS";
            }

            if (statusCode != HttpStatusCode.OK)
            {
                model.ViewName = "Failure_SMS";
            }
        }
    }

    public interface IRegisterForSMSViewModelBuilder
    {
        Task<SMSRegistrationViewModel> CaseDataCaptureApiGenerateVerificationCode(SendSmsOutcomeViewModel model);
        Task<SMSRegistrationViewModel> CaseDataCaptureApiVerifyCode(SendSmsOutcomeViewModel model);
        Task<SMSRegistrationViewModel> CaseDataCaptureApiSubmitSMSRegistration(SendSmsOutcomeViewModel model);
    }
}
