using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromSendSmsOutcomeViewModelToCaseDataCaptureRequest : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SendSmsOutcomeViewModel, CaseDataCaptureRequest>()
                .ConvertUsing<FromSendSmsOutcomeViewModelToCaseDataCaptureRequestConverter>();
        }
    }

    public class FromSendSmsOutcomeViewModelToCaseDataCaptureRequestConverter : ITypeConverter<SendSmsOutcomeViewModel, CaseDataCaptureRequest>
    {
        public CaseDataCaptureRequest Convert(ResolutionContext context)
        {
            var sendSmsOutcomeViewModel = (SendSmsOutcomeViewModel) context.SourceValue;
            var caseDataCaptureRequest = (CaseDataCaptureRequest) context.DestinationValue ?? new CaseDataCaptureRequest();
            
            caseDataCaptureRequest.JourneyId = sendSmsOutcomeViewModel.JourneyId.ToString();
            caseDataCaptureRequest.PostCode = sendSmsOutcomeViewModel.CurrentPostcode;
            caseDataCaptureRequest.Gender = sendSmsOutcomeViewModel.UserInfo.Demography.Gender;
            caseDataCaptureRequest.Age = sendSmsOutcomeViewModel.Age;
            caseDataCaptureRequest.Phone = sendSmsOutcomeViewModel.MobileNumber;
            caseDataCaptureRequest.DaysSinceSymptomsStarted = DateTime.Now.Day - sendSmsOutcomeViewModel.SymptomsStartedDate.Day;
            caseDataCaptureRequest.LiveAlone = sendSmsOutcomeViewModel.LivesAlone;

            return caseDataCaptureRequest;
        }
    }
}
