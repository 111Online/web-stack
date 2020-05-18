using NHS111.Business.DOS.Configuration;
using NHS111.Utils.Monitoring;
using NHS111.Utils.RestTools;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace NHS111.Business.DOS.Api.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public Monitor(ILoggingRestClient restClient, IConfiguration configuration)
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
                var health = await _restClient.ExecuteAsync<bool>(new JsonRestRequest(_configuration.DomainDosApiMonitorHealthUrl, Method.GET));
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