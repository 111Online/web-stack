

using NHS111.Utils.RestTools;

namespace NHS111.Web.Presentation.Builders
{
    using Configuration;
    using NHS111.Models.Models.Web.CCG;
    using RestSharp;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;

    public class CCGViewModelBuilder : ICCGModelBuilder
    {
        private ILoggingRestClient _ccgServiceRestClient;
        private IConfiguration _configuration;
        public CCGViewModelBuilder(ILoggingRestClient ccgServiceRestClient, IConfiguration configuration)
        {
            _ccgServiceRestClient = ccgServiceRestClient;
            _configuration = configuration;
        }


        public async Task<CCGModel> FillCCGModel(string postcode)
        {
            var response = await _ccgServiceRestClient.ExecuteAsync<CCGModel>(
                new RestRequest(_configuration.CCGBusinessApiGetCCGUrl(postcode), Method.GET));

            if (response.Data != null)
                return response.Data;

            return new CCGModel();
        }

        public async Task<CCGDetailsModel> FillCCGDetailsModelAsync(string postCode)
        {
            var response = await _ccgServiceRestClient.ExecuteAsync<CCGDetailsModel>(
                new RestRequest(_configuration.CCGApiGetCCGDetailsByPostcode(postCode), Method.GET));

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException("The supplied postcode was not in a format that the CCG service supports.");
                case HttpStatusCode.NotFound:
                    return new CCGDetailsModel();
                case HttpStatusCode.OK:
                    return response.Data != null ? response.Data : new CCGDetailsModel();
                default:
                    throw new HttpException("CCG Service Error Response");
            }
        }
    }

    public interface ICCGModelBuilder
    {
        Task<CCGModel> FillCCGModel(string postcode);
        Task<CCGDetailsModel> FillCCGDetailsModelAsync(string postCode);
    }
}
