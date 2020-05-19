using NHS111.Business.Configuration;
using NHS111.Utils.Monitoring;
using NHS111.Utils.RestTools;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace NHS111.Business.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public Monitor(ILoggingRestClient restClientDomainApi, IConfiguration configuration)
        {
            _restClient = restClientDomainApi;
            _configuration = configuration;
        }

        public override string Metrics()
        {
            return "Metrics";
        }

        public override async Task<bool> Health()
        {
            try
            {
                var health = await _restClient.ExecuteAsync<bool>(new JsonRestRequest(_configuration.GetDomainApiMonitorHealthUrl(), Method.GET));
                return health.Data;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string Version()
        {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}