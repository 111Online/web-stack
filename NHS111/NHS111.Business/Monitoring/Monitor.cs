using NHS111.Business.Configuration;
using NHS111.Utils.Monitoring;
using NHS111.Utils.RestTools;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace NHS111.Business.Monitoring
{
    using Nest;
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _elasticClient;

        public Monitor(ILoggingRestClient restClientDomainApi, IConfiguration configuration)
        {
            _restClient = restClientDomainApi;
            _configuration = configuration;
            _elasticClient = _configuration.GetElasticClient();
        }

        public override string Metrics()
        {
            return "Metrics";
        }

        public override async Task<bool> Health()
        {
            try
            {
                var elasticHealth = _elasticClient.CatHealthAsync();
                var domainHealth = _restClient.ExecuteAsync<bool>(new JsonRestRequest(_configuration.GetDomainApiMonitorHealthUrl(), Method.GET));

                await Task.WhenAll(elasticHealth, domainHealth).ConfigureAwait(false);

                return elasticHealth.Result.ApiCall.Success && domainHealth.Result.Data;
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
