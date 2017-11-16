using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using RestSharp;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.WhitelistFilter
{
    public class ITKWhitelistFilter : IITKWhitelistFilter
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public ITKWhitelistFilter(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode)
        {
            var localWhiteList = await PopulateLocalCCGITKServiceIdWhitelist(postCode);

            foreach (var service in resultsToFilter)
            {
                service.CallbackEnabled = localWhiteList.Contains(service.Id);
            }

            return resultsToFilter;
        }

        private async Task<List<int>> PopulateLocalCCGITKServiceIdWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<List<int>>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postCode), Method.GET));

            //TODO: Change to CCG object and extract whitelist from it
            return response.Data;
        }
    }

    public interface IITKWhitelistFilter : IWhitelistFilter { }
}
