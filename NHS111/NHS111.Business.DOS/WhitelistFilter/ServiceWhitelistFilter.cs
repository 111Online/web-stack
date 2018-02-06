using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
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

            return resultsToFilter.Where(s => localWhiteList.Contains(s.Id.ToString())).ToList();
        }

        private async Task<ServiceListModel> PopulateLocalCCGServiceIdWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<CCGDetailsModel>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postCode), Method.GET));

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException("CCG Service Error Response");

            if (response.Data != null && response.Data.ServiceIdWhitelist != null)
                return response.Data.ServiceIdWhitelist;
            
            return new ServiceListModel();
        }
    }

    public interface IServiceWhitelistFilter
    {
        Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode);
    }
}
