using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Business.ITKDispatcher.Api.Mappings
{
    public class ItkDispatchResponseBuilder : IItkDispatchResponseBuilder
    {
        private const submitEncounterToServiceResponseOverallStatus SUCCESS_RESPONSE = submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice;
        public ITKDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse)
        {
            return new ITKDispatchResponse(DetermineSuccess(submitHaScToServiceResponse.SubmitEncounterToServiceResponse.OverallStatus));
        }

        private SentStatus DetermineSuccess(submitEncounterToServiceResponseOverallStatus responseStatus)
        {
            if(responseStatus == SUCCESS_RESPONSE) return SentStatus.Success;
            return SentStatus.Failure;
        }
    }

    public interface IItkDispatchResponseBuilder
    {
        ITKDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse);
    }
}