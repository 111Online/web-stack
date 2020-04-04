using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;

namespace NHS111.Models.Mappers.WebMappings
{
    public class DataCaptureApiRequestMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SendSmsOutcomeViewModel, VerifyCodeRequest>()
                .ConvertUsing<FromSendSmsOutcomeViewModelToVerifyCodeRequestConverter>();
            Mapper.CreateMap<SendSmsOutcomeViewModel, SubmitSMSRegistrationRequest>()
                .ConvertUsing<FromSendSmsOutcomeViewModelToSubmitSMSRegistrationRequestConverter>();
        }
    }

    public class FromSendSmsOutcomeViewModelToVerifyCodeRequestConverter : ITypeConverter<SendSmsOutcomeViewModel, VerifyCodeRequest>
    {
        public VerifyCodeRequest Convert(ResolutionContext context)
        {
            var Model = (SendSmsOutcomeViewModel)context.SourceValue;
            var verifyCodeRequest = (VerifyCodeRequest)context.DestinationValue ?? new VerifyCodeRequest();

            verifyCodeRequest.MobilePhoneNumber = Model.MobileNumber;
            verifyCodeRequest.VerificationCodeInput = Model.VerificationCodeInput.InputValue;

            return verifyCodeRequest;
        }
    }

    public class FromSendSmsOutcomeViewModelToSubmitSMSRegistrationRequestConverter : ITypeConverter<SendSmsOutcomeViewModel, SubmitSMSRegistrationRequest>
    {
        public SubmitSMSRegistrationRequest Convert(ResolutionContext context)
        {
            var Model = (SendSmsOutcomeViewModel) context.SourceValue;
            var submitSmsRegistrationRequest = (SubmitSMSRegistrationRequest) context.DestinationValue ?? new SubmitSMSRegistrationRequest();
            
            submitSmsRegistrationRequest.JourneyId = Model.JourneyId.ToString();
            submitSmsRegistrationRequest.PostCode = Model.CurrentPostcode;
            submitSmsRegistrationRequest.Age = Model.Age;
            submitSmsRegistrationRequest.Phone = Model.MobileNumber;
            var daysAgo = Model.SymptomsStartedDaysAgo;
            var symptomsStartedDate = DateTime.Now.AddDays(daysAgo * -1);
            submitSmsRegistrationRequest.SymptomsStarted = symptomsStartedDate.ToString("yyyy-MM-dd");
            submitSmsRegistrationRequest.LiveAlone = Model.LivesAlone;
            submitSmsRegistrationRequest.VerificationCodeInput = Model.VerificationCodeInput.InputValue;

            return submitSmsRegistrationRequest;
        }
    }
}
