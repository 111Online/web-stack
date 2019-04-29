using System;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Utils.Monitoring;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public Monitor(IRestClient restClientDomainApi, IConfiguration configuration)
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
                var health = await _restClient.ExecuteTaskAsync<bool>(new JsonRestRequest(_configuration.GetDomainApiMonitorHealthUrl(), Method.GET));
                return health.Data;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override string Version() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}