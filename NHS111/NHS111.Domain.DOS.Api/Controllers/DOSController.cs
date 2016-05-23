using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Domain.DOS.Api.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Utils.Attributes;
using NHS111.Utils.Helpers;

namespace NHS111.Domain.DOS.Api.Controllers
{
    [LogHandleErrorForApi]
    public class DOSController : ApiController
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public DOSController(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("DOSapi/CheckCapacitySummary")]
        public async Task<HttpResponseMessage> CheckCapacitySummary(HttpRequestMessage request)
        {
            return await _restfulHelper.PostAsync(_configuration.DOSIntegrationCheckCapacitySummaryUrl, request);
        }

        [HttpPost]
        [Route("DOSapi/ServiceDetailsById")]
        public async Task<HttpResponseMessage> ServiceDetailsById(HttpRequestMessage request)
        {
            return await _restfulHelper.PostAsync(_configuration.DOSIntegrationServiceDetailsByIdUrl, request);
        }

        [HttpPost]
        [Route("DOSapi/ServicesByClinicalTerm")]
        public async Task<string> ServicesByClinicalTerm(DosServicesByClinicalTermRequest request)
        {
            var data = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}", request.CaseId, request.Postcode, request.SearchDistance, request.GpPracticeId, request.Age, request.Gender, request.Disposition, request.SymptomGroupDiscriminatorCombos, request.NumberPerType);

            var url = string.Format("{0}{1}", _configuration.DOSMobileIntegrationServicesByClinicalTermUrl, data);

            return await _restfulHelper.GetAsync(url);
        }
    }
}
