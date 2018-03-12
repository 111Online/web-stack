using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.FromExternalServices.IdealPostcodes;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class LocationResultBuilder : ILocationResultBuilder
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IRestClient _restLocationService;
        private readonly IConfiguration _configuration;
        private const string SubscriptionKey = "Ocp-Apim-Subscription-Key";

        public LocationResultBuilder(IRestfulHelper restfulHelper, IRestClient restLocationService, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _restLocationService = restLocationService;
        }

        public async Task<List<AddressLocationResult>> LocationResultByPostCodeBuilder(string postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return new List<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<List<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetAddressByPostcodeUrl(postCode), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return JsonConvert.DeserializeObject<List<AddressLocationResult>>(response.Content);
            throw response.ErrorException;
        }

        public async Task<List<AddressLocationResult>> LocationResultByGeouilder(string longlat)
        {
            if (string.IsNullOrEmpty(longlat)) return new List<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<List<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetAddressByGeoUrl(longlat), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return JsonConvert.DeserializeObject<List<AddressLocationResult>>(response.Content);
            throw response.ErrorException;
        }
    }

    public interface ILocationResultBuilder
    {
        Task<List<AddressLocationResult>> LocationResultByPostCodeBuilder(string postCode);
        Task<List<AddressLocationResult>> LocationResultByGeouilder(string longlat);
    }
}