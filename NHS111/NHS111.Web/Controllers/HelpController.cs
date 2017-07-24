using System.Web.Mvc;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HelpController : Controller
    {
        public HelpController(IConfiguration configuration, IRestClient restClientBusinessApi)
        {
            _configuration = configuration;
            _restClientBusinessApi = restClientBusinessApi;
        }

        public ActionResult Cookies()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Browsers()
        {
            return View();
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