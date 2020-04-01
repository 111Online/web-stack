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
            caseDataCaptureRequest.Age = sendSmsOutcomeViewModel.Age;
            caseDataCaptureRequest.Phone = sendSmsOutcomeViewModel.MobileNumber;
            var daysAgo = sendSmsOutcomeViewModel.SymptomsStartedDaysAgo;
            var symptomsStartedDate = DateTime.Now.AddDays(daysAgo * -1);
            caseDataCaptureRequest.SymptomsStarted = symptomsStartedDate.ToString("yyyy-MM-dd");
            caseDataCaptureRequest.LiveAlone = sendSmsOutcomeViewModel.LivesAlone;

            return caseDataCaptureRequest;
        }
    }
}
