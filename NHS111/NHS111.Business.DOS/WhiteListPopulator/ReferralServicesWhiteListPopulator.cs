﻿using System.Net;
using System.Threading.Tasks;
using System.Web;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
using RestSharp;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public class ReferralServicesWhiteListPopulator : IWhiteListPopulator
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public ReferralServicesWhiteListPopulator(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<ServiceListModel> PopulateCCGWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<CCGDetailsModel>(
                new RestRequest(string.Format(_configuration.CCGApiGetCCGDetailsByPostcode, postCode), Method.GET));

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException("CCG Service Error Response");

            if (response.Data != null && response.Data.ReferralServiceIdWhitelist != null)
                return response.Data.ReferralServiceIdWhitelist;

            return new ServiceListModel();
        }

    }
}
