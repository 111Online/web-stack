using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class VersionController : Controller
    {
        public VersionController(IConfiguration configuration, IRestClient restClientBusinessApi)
        {
            _configuration = configuration;
            _restClientBusinessApi = restClientBusinessApi;
        }

        public ActionResult VersionInfo()
        {
            var url = _configuration.GetBusinessApiVersionUrl(true);
            var request = new RestRequest(url, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _restClientBusinessApi.Execute<VersionInfo>(request);

            return PartialView("_VersionInfo", response.Data);
        }

        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClientBusinessApi;
    }
}