using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Location;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Builders
{
    public class LocationResultBuilder : ILocationResultBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restLocationService;

        public LocationResultBuilder(IConfiguration configuration, IRestClient restLocationService)
        {

            _configuration = configuration;
            _restLocationService = restLocationService;
        }

        public async Task<List<AddressLocationResult>> LocationResultByPostCodeBuilder(string postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return new List<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<List<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetAddressByPostcodeUrl(postCode, true), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data;
            throw response.ErrorException;
        }

        public async Task<List<AddressLocationResult>> LocationResultByGeouilder(string longlat)
        {
            if (string.IsNullOrEmpty(longlat)) return new List<AddressLocationResult>();
            var response = await _restLocationService.ExecuteTaskAsync<List<AddressLocationResult>>(
                new RestRequest(_configuration.GetBusinessApiGetAddressByGeoUrl(longlat, true), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data;
            throw response.ErrorException;
        }
    }

    public interface ILocationResultBuilder
    {
        Task<List<AddressLocationResult>> LocationResultByPostCodeBuilder(string postCode);
        Task<List<AddressLocationResult>> LocationResultByGeouilder(string longlat);
    }
}