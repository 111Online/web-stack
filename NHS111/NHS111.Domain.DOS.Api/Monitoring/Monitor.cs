using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Domain.DOS.Api.Configuration;
using NHS111.Utils.Helpers;
using NHS111.Utils.Monitoring;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Domain.DOS.Api.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public Monitor(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
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
                var health = await _restClient.ExecuteTaskAsync<bool>(new JsonRestRequest(_configuration.DOSIntegrationMonitorHealthUrl, Method.GET));
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