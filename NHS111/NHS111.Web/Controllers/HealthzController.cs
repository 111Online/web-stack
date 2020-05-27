using NHS111.Web.Authentication;
using NHS111.Web.Presentation.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NHS111.Web.Controllers
{
    public class HealthzController : ApiController
    {
        private readonly HttpClient _httpClient = new HttpClient(); // using classic HttpClient here to avoid larger changes in IoC

        private readonly Uri CCGApiMonitorUrl;
        private readonly Uri BusinessApiMonitorUrl;
        private readonly Uri BusinessDosApiMonitorUrl;

        public HealthzController(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            CCGApiMonitorUrl = new Uri(configuration.CCGBusinessApiBaseProtocolandDomain + configuration.GetCCGApiMonitorHealthUrl());
            BusinessApiMonitorUrl = new Uri(configuration.BusinessApiProtocolandDomain + configuration.GetBusinessApiMonitorHealthUrl());
            BusinessDosApiMonitorUrl = new Uri(configuration.BusinessDosApiBaseUrl + configuration.GetBusinessDosApiMonitorHealthUrl());
        }

        /// <summary>
        /// This health endpoint is used by FrontDoor
        /// It calls downstream Business API's monitor endpoint - which in turn checks Domain API which checks Neo4J
        /// </summary>
        /// <returns></returns>
        [AllowAnonymousAccess]
        [HttpGet]
        [HttpHead]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var healthTasks = new Task<string>[]
                {
                    _httpClient.GetStringAsync(BusinessApiMonitorUrl),
                    //_httpClient.GetStringAsync(BusinessDosApiMonitorUrl), // TODO: DOS Api failing now, investigate and uncomment when DOS ready
                    _httpClient.GetStringAsync(CCGApiMonitorUrl)
                };

                await Task.WhenAll(healthTasks).ConfigureAwait(false);

                var healthy = true;
                foreach (var t in healthTasks)
                {
                    healthy = (!string.IsNullOrEmpty(t.Result) && t.Result.ToLower().Contains("true"));
                    if (healthy == false)
                    {
                        // One of the dependencies of "first line" APIs is not healthy. No need to continue.
                        return StatusCode(HttpStatusCode.ServiceUnavailable);
                    }
                }

                return Ok("healthy");
            }
            catch (Exception)
            {
                // One of the APIs called directly from Web is not healthy.
                return StatusCode(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}
