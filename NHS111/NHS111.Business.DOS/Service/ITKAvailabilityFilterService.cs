using System.Collections.Generic;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using RestSharp;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class ITKAvailabilityFilterService : IITKAvailabilityFilterService
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public ITKAvailabilityFilterService(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode)
        {
            var blackList = await PopulateITKServiceIdBlacklist(postCode);
            var localWhiteList = await PopulateLocalCCGITKServiceIdWhitelist(postCode);

            resultsToFilter.RemoveAll(s => blackList.Contains(s.Id));

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

        private async Task<List<int>> PopulateITKServiceIdBlacklist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<List<int>>(
                new RestRequest(string.Format(_configuration.CCGApiGetServiceIdBlacklist, postCode), Method.GET));

            return response.Data;
        }
    }

    public interface IITKAvailabilityFilterService
    {
        Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode);
    }
}
