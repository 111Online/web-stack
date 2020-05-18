using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System.Web.Mvc;
using NHS111.Utils.RestTools;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class VersionController : Controller
    {
        public VersionController(IConfiguration configuration, ILoggingRestClient restClientBusinessApi)
        {
            _configuration = configuration;
            _restClientBusinessApi = restClientBusinessApi;
        }

        public async Task<ActionResult> VersionInfo()
        {
            var url = _configuration.GetBusinessApiVersionUrl(true);
            var request = new RestRequest(url, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = await _restClientBusinessApi.ExecuteAsync<VersionInfo>(request).ConfigureAwait(false);

            return PartialView("_VersionInfo", response.Data);
        }

        private readonly IConfiguration _configuration;
        private readonly ILoggingRestClient _restClientBusinessApi;
    }
}