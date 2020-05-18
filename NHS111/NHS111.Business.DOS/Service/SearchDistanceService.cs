using Newtonsoft.Json;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using NHS111.Utils.RestTools;

namespace NHS111.Business.DOS.Service
{
    public class SearchDistanceService : ISearchDistanceService
    {
        private readonly ILoggingRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public SearchDistanceService(ILoggingRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<int> GetSearchDistanceByPostcode(string postcode)
        {
            var response = await _restCCGApi.ExecuteAsync(new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postcode), Method.GET));

            int dosSearchDistance;
            if (response.StatusCode != HttpStatusCode.OK || response.Content == null) return _configuration.DoSSearchDistance;

            var ccg = JsonConvert.DeserializeObject<CCGModel>(response.Content);
            if (ccg == null) return _configuration.DoSSearchDistance;

            return int.TryParse(ccg.SearchDistance, out dosSearchDistance) ? dosSearchDistance : _configuration.DoSSearchDistance;
        }
    }

    public interface ISearchDistanceService
    {
        Task<int> GetSearchDistanceByPostcode(string postcode);
    }
}
