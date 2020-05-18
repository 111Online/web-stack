using NHS111.Utils.Attributes;
using NHS111.Utils.Monitoring;
using System.Threading.Tasks;
using System.Web.Http;

namespace NHS111.Domain.Api.Controllers
{
    [LogHandleErrorForApi]
    public class MonitoringController : ApiController
    {
        private readonly IMonitor _monitor;

        public MonitoringController(IMonitor monitor)
        {
            _monitor = monitor;
        }

        [HttpGet]
        [Route("monitor/{service}")]
        public async Task<string> MonitorPing(string service)
        {
            switch (service.Trim().ToLower())
            {
                case "ping":
                    return _monitor.Ping();

                case "metrics":
                    return _monitor.Metrics();

                case "health":
                    return (await _monitor.Health()).ToString();

                case "version":
                    return _monitor.Version();

                default:
                    return null;
            }
        }
    }
}