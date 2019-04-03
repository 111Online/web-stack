using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using CheckCapacitySummaryResult = NHS111.Models.Models.Business.CheckCapacitySummaryResult;

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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("DOSapi/CheckCapacitySummary")]
        public async Task<JsonResult<DosCheckCapacitySummaryResult>> CheckCapacitySummary([FromBody]DosFilteredCase dosFilteredCase, [FromUri]string endpoint = null, [FromUri]bool filterServices = true) {
            var dosEndpoint = EnumHelper.ParseEnum<DosEndpoint>(endpoint, DosEndpoint.Unspecified);
            return Json(await _serviceAvailabilityFilterService.GetFilteredServices(dosFilteredCase, filterServices, dosEndpoint));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("DOSapi/ServiceDetailsById")]
        public async Task<JsonResult<ServiceDetailsByIdResponse>> ServiceDetailsById([FromBody]DosServiceDetailsByIdRequest serviceDetailsByIdRequest)
        {
            return Json(await _dosService.GetServiceById(serviceDetailsByIdRequest));
        }
    }
}
