
using NHS111.Models.Models.Domain;
using NHS111.Utils.RestTools;
using RestSharp;
using System.Collections.Generic;

namespace NHS111.Business.Services
{

    using Configuration;
    using System.Threading.Tasks;

    public class OutcomeService
        : IOutcomeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggingRestClient _restClient;

        public OutcomeService(IConfiguration configuration, ILoggingRestClient restClientDomainApi)
        {
            _configuration = configuration;
            _restClient = restClientDomainApi;
        }

        public async Task<IEnumerable<Outcome>> List()
        {
            var outcomes = await _restClient.ExecuteAsync<IEnumerable<Outcome>>(new JsonRestRequest(_configuration.GetDomainApiListOutcomesUrl(), Method.GET));
            return outcomes.Data;
        }
    }

    public interface IOutcomeService
    {
        Task<IEnumerable<Outcome>> List();
    }
}