using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NHS111.Models.Models.Web.CCG;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class CCGViewModelBuilder : ICCGModelBuilder
    {
        private IRestClient _ccgServiceRestClient;
        private IConfiguration _configuration;
        public CCGViewModelBuilder(IRestClient ccgServiceRestClient, IConfiguration configuration)
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

    public interface ICCGModelBuilder
    {
        Task<CCGModel> FillCCGModel(string postcode);
    }

    //public interface ICCGApiRestClient : IRestClient { }

    //public class LoggingCCGApiRestClient : LoggingRestClient, ICCGApiRestClient
    //{
    //    public LoggingCCGApiRestClient(string baseUrl, ILog logger) : base(baseUrl, logger) { }
    //}
 }
