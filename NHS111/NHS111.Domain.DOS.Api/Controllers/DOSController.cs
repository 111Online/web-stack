using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NHS111.Domain.DOS.Api.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Utils.Attributes;
using NHS111.Utils.Helpers;

namespace NHS111.Domain.DOS.Api.Controllers
{
    using Utils.Extensions;

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
        public async Task<HttpResponseMessage> ServicesByClinicalTerm(HttpRequestMessage request)
        {
            var requestObj = JsonConvert.DeserializeObject<DosServicesByClinicalTermRequest>(request.Content.ReadAsStringAsync().Result);

            var urlWithRequest = string.Format(_configuration.DOSMobileServicesByClinicalTermUrl, requestObj.CaseId, requestObj.Postcode, requestObj.SearchDistance, requestObj.GpPracticeId, requestObj.Age, requestObj.Gender, requestObj.Disposition, requestObj.SymptomGroupDiscriminatorCombos, requestObj.NumberPerType);

            var usernamePassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.DOSMobileUsername + ":" + _configuration.DOSMobilePassword));
            var headers = new Dictionary<string, string>() { { HttpRequestHeader.Authorization.ToString(), string.Format("Basic {0}", usernamePassword) } };
            var result = await _restfulHelper.GetAsync(urlWithRequest, headers);

            return result.AsHttpResponse();
        }
    }
}
