using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NHS111.Integration.DOS.Api.DOSService;
using NHS111.Utils.Attributes;

namespace NHS111.Integration.DOS.Api.Controllers
{
    [LogHandleErrorForApi]
    public class DOSController : ApiController
    {
        private readonly PathWayServiceSoap _pathWayServiceSoap;

        public DOSController(PathWayServiceSoap pathWayServiceSoap)
        {
            _pathWayServiceSoap = pathWayServiceSoap;
        }

        [HttpPost]
        [Route("IntegrationDOSapi/CheckCapacitySummary")]
        public async Task<CheckCapacitySummaryResponse> CheckCapacitySummary(HttpRequestMessage request)
        {
            var jsonString = await request.Content.ReadAsStringAsync();
            var checkCapacitySummaryRequest = JsonConvert.DeserializeObject<CheckCapacitySummaryRequest>(jsonString);
            return await _pathWayServiceSoap.CheckCapacitySummaryAsync(checkCapacitySummaryRequest);
        }

        [HttpPost]
        [Route("IntegrationDOSapi/ServiceDetailsById")]
        public async Task<ServiceDetailsByIdResponse> ServiceDetailsById(HttpRequestMessage request)
        {
            var jsonString = await request.Content.ReadAsStringAsync();
            var serviceDetailsByIdRequest = JsonConvert.DeserializeObject<ServiceDetailsByIdRequest>(jsonString);
            return await _pathWayServiceSoap.ServiceDetailsByIdAsync(serviceDetailsByIdRequest);
        }
    }
}
