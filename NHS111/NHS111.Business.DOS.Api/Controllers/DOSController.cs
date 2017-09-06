using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.DOS.Service;
using NHS111.Utils.Attributes;

namespace NHS111.Business.DOS.Api.Controllers
{
    using System;
    using Models.Models.Web.DosRequests;
    using Utils.Helpers;

    [LogHandleErrorForApi]
    public class DOSController : ApiController
    {
        private readonly IServiceAvailabilityFilterService _serviceAvailabilityFilterService;
        private readonly IDosService _dosService;

        public DOSController(IDosService dosService, IServiceAvailabilityFilterService serviceAvailabilityFilterService)
        {
            _serviceAvailabilityFilterService = serviceAvailabilityFilterService;
            _dosService = dosService;
        }

        [HttpPost]
        [Route("DOSapi/CheckCapacitySummary")]
        public async Task<HttpResponseMessage> CheckCapacitySummary(HttpRequestMessage request, string endpoint = null, [FromUri] bool filterServices = true) {
            var dosEndpoint = EnumHelper.ParseEnum<DosEndpoint>(endpoint, DosEndpoint.Unspecified);
            return await _serviceAvailabilityFilterService.GetFilteredServices(request, filterServices, dosEndpoint);
        }

        [HttpPost]
        [Route("DOSapi/ServiceDetailsById")]
        public async Task<HttpResponseMessage> ServiceDetailsById(HttpRequestMessage request)
        {
            return await _dosService.GetServiceById(request);
        }

        [HttpPost]
        [Route("DOSapi/ServicesByClinicalTerm")]
        public async Task<HttpResponseMessage> ServicesByClinicalTerm(HttpRequestMessage request)
        {
            return await _dosService.GetServicesByClinicalTerm(request);
        }
    }
}
