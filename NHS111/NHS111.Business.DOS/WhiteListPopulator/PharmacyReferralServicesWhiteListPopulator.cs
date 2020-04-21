using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using NHS111.Utils.RestTools;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public class PharmacyReferralServicesWhiteListPopulator : IWhiteListPopulator
    {
        private readonly ILoggingRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public PharmacyReferralServicesWhiteListPopulator(ILoggingRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<ServiceListModel> PopulateCCGWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteAsync<CCGDetailsModel>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGDetailsByPostcode, postCode), Method.GET));

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException("CCG Service Error Response");

            if (response.Data != null && response.Data.PharmacyReferralServiceIdWhitelist != null)
                return response.Data.PharmacyReferralServiceIdWhitelist;

            return new ServiceListModel();
        }

    }
}
