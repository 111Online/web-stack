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
    public class CCGService : ICCGService
    {
        private IRestClient _ccgServiceRestClient;
        private IConfiguration _configuration;

        public CCGService(IRestClient ccgServiceRestClient, IConfiguration configuration)
        {
            _ccgServiceRestClient = ccgServiceRestClient;
            _configuration = configuration;
        }


        public async Task<CCGModel> FillCCGModel(string postcode)
        {
            var response = await _ccgServiceRestClient.ExecuteTaskAsync<CCGModel>(
                new RestRequest(_configuration.CCGBusinessApiGetCCGUrl(postcode), Method.GET));

            if (response.Data != null && response.Data != null)
                return response.Data;

            return new CCGModel();
        }
    }

    public interface ICCGService
    {
        Task<CCGModel> FillCCGModel(string postcode);
    }
}