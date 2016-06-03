
namespace NHS111.Business.Services {

    using System.Threading.Tasks;
    using Configuration;
    using Utils.Helpers;

    public class OutcomeService
        : IOutcomeService {
        
        public OutcomeService(IConfiguration configuration, IRestfulHelper restfulHelper) {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }

        public async Task<string> List() {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiListOutcomesUrl());
        }

        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;
    }

    public interface IOutcomeService {
        Task<string> List();
    }
}