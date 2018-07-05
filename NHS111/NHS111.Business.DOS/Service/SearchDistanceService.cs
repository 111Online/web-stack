using System;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
using RestSharp;

namespace NHS111.Business.DOS.Service
{
    public class SearchDistanceService : ISearchDistanceService
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public SearchDistanceService(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<int> GetSearchDistanceByPostcode(string postcode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<CCGModel>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postcode), Method.GET));

            int dosSearchDistance;
            if (response.Data != null)
                return int.TryParse(response.Data.SearchDistance, out dosSearchDistance) ? dosSearchDistance : _configuration.DoSSearchDistance;

            return _configuration.DoSSearchDistance;
        }
    }

    public interface ISearchDistanceService
    {
        Task<int> GetSearchDistanceByPostcode(string postcode);
    }
}
