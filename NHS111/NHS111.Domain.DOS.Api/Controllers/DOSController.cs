using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using NHS111.Domain.DOS.Api.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Domain.DOS.Api.Controllers
{
    using System.Web;
    using Utils.Extensions;

    [LogHandleErrorForApi]
    public class DOSController : ApiController
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public DOSController(IRestClient restClientDosProxyDomainApi, IConfiguration configuration)
        {
            _restClient = restClientDosProxyDomainApi;
            _configuration = configuration;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("DOSapi/CheckCapacitySummary")]
        public async Task<JsonResult<CheckCapacitySummaryResponse>> CheckCapacitySummary([FromBody]DosCheckCapacitySummaryRequest dosRequest, [FromUri]string endpoint = "")
        {
            var url = string.Format("{0}?endpoint={1}", _configuration.DOSIntegrationCheckCapacitySummaryUrl, string.IsNullOrEmpty(endpoint) ? "Unspecified" : endpoint);
            var request = new JsonRestRequest(url, Method.POST);
            request.AddJsonBody(dosRequest);

            var response = await _restClient.ExecuteTaskAsync<CheckCapacitySummaryResponse>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
                return Json(response.Data);
            throw response.ErrorException;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("DOSapi/ServiceDetailsById")]
        public async Task<JsonResult<ServiceDetailsByIdResponse>> ServiceDetailsById([FromBody]DosServiceDetailsByIdRequest dosRequest)
        {
            var request = new JsonRestRequest(_configuration.DOSIntegrationServiceDetailsByIdUrl, Method.POST);
            request.AddJsonBody(dosRequest);

            var response = await _restClient.ExecuteTaskAsync<ServiceDetailsByIdResponse>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
                return Json(response.Data);
            throw response.ErrorException;
        }
    }
}
