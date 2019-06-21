
using System.Collections.Generic;
using NHS111.Models.Models.Domain;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Services {

    using System.Threading.Tasks;
    using Configuration;

    public class OutcomeService
        : IOutcomeService {
        
        public OutcomeService(IConfiguration configuration, IRestClient restClientDomainApi) {
            _configuration = configuration;
            _restClient = restClientDomainApi;
        }

        public async Task<IEnumerable<Outcome>> List()
        {
            var outcomes = await _restClient.ExecuteTaskAsync<IEnumerable<Outcome>>(new JsonRestRequest(_configuration.GetDomainApiListOutcomesUrl(), Method.GET));
            return outcomes.Data;
        }

        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
    }

    public interface IOutcomeService {
        Task<IEnumerable<Outcome>> List();
    }
}