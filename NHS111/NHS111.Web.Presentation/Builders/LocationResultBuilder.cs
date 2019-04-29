using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Location;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using AddressLocationResult = NHS111.Models.Models.Web.FromExternalServices.IdealPostcodes.AddressLocationResult;

namespace NHS111.Web.Presentation.Builders
{
    public class LocationResultBuilder : ILocationResultBuilder
    {
        private readonly IRestClient _restLocationService;
        private readonly IConfiguration _configuration;
        private const string SubscriptionKey = "Ocp-Apim-Subscription-Key";

        public LocationResultBuilder(IRestClient restLocationService, IConfiguration configuration)
        {
            _configuration = configuration;
            _restLocationService = restLocationService;
        }

        public async Task<List<AddressLocationResult>> LocationResultByPostCodeBuilder(string postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return new List<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<List<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetAddressByPostcodeUrl(postCode), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed )
                return JsonConvert.DeserializeObject<List<AddressLocationResult>>(response.Content);
            throw response.ErrorException;
        }


        public async Task<LocationServiceResult<AddressLocationResult>> LocationResultValidatedByPostCodeBuilder(string postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return new LocationServiceResult<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<LocationServiceResult<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetValidatedAddressByPostcodeUrl(postCode), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                      return JsonConvert.DeserializeObject<LocationServiceResult<AddressLocationResult>>(response.Content);
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
        Task<LocationServiceResult<AddressLocationResult>> LocationResultValidatedByPostCodeBuilder(string postCode);
        Task<List<AddressLocationResult>> LocationResultByGeouilder(string longlat);
    }
}