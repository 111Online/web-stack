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
        private readonly IPathwayServiceSoapFactory _pathWayServiceFactory;

        public DOSController(IPathwayServiceSoapFactory pathWayServiceFactory)
        {
            _pathWayServiceFactory = pathWayServiceFactory;
        }

        [HttpPost]
        [Route("IntegrationDOSapi/CheckCapacitySummary")]
        public async Task<CheckCapacitySummaryResponse> CheckCapacitySummary(HttpRequestMessage request)
        {
            var jsonString = await request.Content.ReadAsStringAsync();
            var checkCapacitySummaryRequest = JsonConvert.DeserializeObject<CheckCapacitySummaryRequest>(jsonString);
            var client = _pathWayServiceFactory.Create(request);
            
            return await client.CheckCapacitySummaryAsync(checkCapacitySummaryRequest);
        }

        [HttpPost]
        [Route("IntegrationDOSapi/ServiceDetailsById")]
        public async Task<ServiceDetailsByIdResponse> ServiceDetailsById(HttpRequestMessage request)
        {
            var jsonString = await request.Content.ReadAsStringAsync();
            var serviceDetailsByIdRequest = JsonConvert.DeserializeObject<ServiceDetailsByIdRequest>(jsonString);
            var client = _pathWayServiceFactory.Create(request);
            return await client.ServiceDetailsByIdAsync(serviceDetailsByIdRequest);
        }
    }
}
