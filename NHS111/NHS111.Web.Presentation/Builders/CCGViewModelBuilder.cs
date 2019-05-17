

namespace NHS111.Web.Presentation.Builders
{
    using System;
    using System.Net;
    using System.Web;
    using System.Threading.Tasks;
    using NHS111.Models.Models.Web.CCG;
    using Configuration;
    using RestSharp;

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

            if (response.Data != null)
                return response.Data;

            return new CCGModel();
        }

        public async Task<CCGDetailsModel> FillCCGDetailsModelAsync(string postCode)
        {
            var response = await _ccgServiceRestClient.ExecuteTaskAsync<CCGDetailsModel>(
                new RestRequest(_configuration.CCGApiGetCCGDetailsByPostcode(postCode), Method.GET));

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("The supplied postcode was not in a format that the CCG service supports.");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException("CCG Service Error Response");


            if (response.Data != null)
                return response.Data;

            return new CCGDetailsModel();
        }

    }

    public interface ICCGModelBuilder
    {
        Task<CCGModel> FillCCGModel(string postcode);
        Task<CCGDetailsModel> FillCCGDetailsModelAsync(string postCode);
    }

    //public interface ICCGApiRestClient : IRestClient { }

    //public class LoggingCCGApiRestClient : LoggingRestClient, ICCGApiRestClient
    //{
    //    public LoggingCCGApiRestClient(string baseUrl, ILog logger) : base(baseUrl, logger) { }
    //}
}
