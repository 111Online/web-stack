using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using RestSharp;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.WhitelistFilter
{
    public class ServiceWhitelistFilter : IServiceWhitelistFilter
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public ServiceWhitelistFilter(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode)
        {
            var localWhiteList = await PopulateLocalCCGServiceIdWhitelist(postCode);
            if (localWhiteList.Count == 0) return resultsToFilter;

            return resultsToFilter.Where(s => localWhiteList.Contains(s.Id)).ToList();
        }

        private async Task<List<int>> PopulateLocalCCGServiceIdWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<List<int>>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postCode), Method.GET));

            //TODO: Change to CCG object and extract whitelist from it
            return response.Data;
        }
    }
    public interface IServiceWhitelistFilter : IWhitelistFilter { }
}
