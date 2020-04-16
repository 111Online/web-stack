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

        public async Task<SMSRegistrationViewModel> MessageCaseDataCaptureApi<TRequest, TViewDeterminer>(SendSmsOutcomeViewModel model, string endPoint) 
            where TViewDeterminer : SMSViewDeterminerBase, new()
        {
            var request = Mapper.Map<TRequest>(model);

            var response = await SendRequestToDataCaptureApi<TRequest>(endPoint, request);

            var smsRegisrationModel = new SMSRegistrationViewModel(model);

            new TViewDeterminer().Build(smsRegisrationModel, response.StatusCode);

            return smsRegisrationModel;
        }


        private async Task<IRestResponse> SendRequestToDataCaptureApi<T>(string endPoint, T body)
        {
            var request = new JsonRestRequest(endPoint, Method.POST);

            request.AddJsonBody(body);

            return await _restClientCaseDataCaptureApi.ExecuteTaskAsync(request);
        }
    }

    public interface IRegisterForSMSViewModelBuilder
    {
        Task<SMSRegistrationViewModel> MessageCaseDataCaptureApi<TRequest, TViewDeterminer>(
            SendSmsOutcomeViewModel model, string endPoint) where TViewDeterminer : SMSViewDeterminerBase, new();
    }

    public abstract class SMSViewDeterminerBase
    {
        protected string SuccessView { get; set; }
        public void BaseBuild(SMSRegistrationViewModel model, HttpStatusCode statusCode)
        {
            model.ViewName = SuccessView;

            if (statusCode != HttpStatusCode.OK)
            {
                model.ViewName = "Failure_SMS";
            }

            if ((int)statusCode == 429)
            {
                model.ViewName = "Too_Many_Requests_SMS";
            }
        }

        public virtual void Build(SMSRegistrationViewModel model, HttpStatusCode statusCode)
        {
            BaseBuild(model, statusCode);
        }
    }

    public class SMSGenerateCodeViewDeterminer : SMSViewDeterminerBase
    {
        public SMSGenerateCodeViewDeterminer()
        {
            SuccessView = "Enter_Verification_Code_SMS";
        }
    }

    public class SMSEnterVerificationCodeViewDeterminer : SMSViewDeterminerBase
    {
        public SMSEnterVerificationCodeViewDeterminer()
        {
            SuccessView = "";
        }
        public override void Build(SMSRegistrationViewModel model, HttpStatusCode statusCode)
        {
            base.BaseBuild(model, statusCode);
        }
    }

    public class SMSSubmitRegistrationViewDeterminer : SMSViewDeterminerBase
    {
        public SMSSubmitRegistrationViewDeterminer()
        {
            SuccessView = "Confirmation_SMS";
        }
    }
}
