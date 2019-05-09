using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Web.CCG;
using RestSharp;

namespace NHS111.Business.Services
{
    public class CCGDetailsService : ICCGDetailsService
    {
        private IRestClient _ccgServiceRestClient;
        private IConfiguration _configuration;

        public CCGDetailsService(IRestClient ccgServiceRestClient, IConfiguration configuration)
        {
            _ccgServiceRestClient = ccgServiceRestClient;
            _configuration = configuration;
        }


        public async Task<CCGDetailsModel> FillCCGDetailsModel(string postcode)
        {
            var response = await _ccgServiceRestClient.ExecuteTaskAsync<CCGDetailsModel>(
                new RestRequest(_configuration.CCGBusinessApiGetCCGUrl(postcode), Method.GET));

            if (response.Data != null && response.Data != null)
                return response.Data;

            return new CCGDetailsModel();
        }
    }

    public interface ICCGDetailsService
    {
        Task<CCGDetailsModel> FillCCGDetailsModel(string postcode);
    }
}