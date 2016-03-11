
namespace NHS111.Business.ITKDispatcher.Api.Mappings
{
    using System.Net;
    using ITKDispatcherSOAPService;
    using Models.Models.Web.ITK;

    public class ItkDispatchResponseBuilder : IItkDispatchResponseBuilder
    {
        private const submitEncounterToServiceResponseOverallStatus SUCCESS_RESPONSE = submitEncounterToServiceResponseOverallStatus.Successful_call_to_gp_webservice;
        public ITKDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse)
        {
            return new ITKDispatchResponse {
                StatusCode = DetermineSuccess(submitHaScToServiceResponse.SubmitEncounterToServiceResponse.OverallStatus)
            };
        }

        // Suggest mapping this an automapper mapping: submitEncounterToServiceResponseOverallStatus -> HttpStatusCode
        private HttpStatusCode DetermineSuccess(submitEncounterToServiceResponseOverallStatus responseStatus)
        {
            if(responseStatus == SUCCESS_RESPONSE) return HttpStatusCode.OK;
            return HttpStatusCode.InternalServerError;
        }
    }

    public interface IItkDispatchResponseBuilder
    {
        ITKDispatchResponse Build(SubmitHaSCToServiceResponse submitHaScToServiceResponse);
    }
}