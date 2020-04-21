using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.Location;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHS111.Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRestClient _restIdealPostcodesApi;
        private readonly IConfiguration _configuration;

        public LocationService(IRestClient restIdealPostcodesApi, IConfiguration configuration)
        {
            _restIdealPostcodesApi = restIdealPostcodesApi;
            _configuration = configuration;
        }

        public async Task<List<PostcodeLocationResult>> FindPostcodes(double longitude, double latitude)
        {
            var response = await _restIdealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<PostcodeLocationResult>>(
                new RestRequest(_configuration.GetLocationPostcodebyGeoUrl(longitude, latitude), Method.GET));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result.ToList();
            throw response.ErrorException;
        }

        public async Task<List<AddressLocationResult>> FindAddresses(double longitude, double latitude)
        {
            var postcodes = await FindPostcodes(longitude, latitude);
            if (postcodes.Count > 0)
                return await FindAddresses(postcodes.First().PostCode);
            return new List<AddressLocationResult>();
        }

        public async Task<AddressLocationSingleResult> FindAddressFromUDPRN(string udprn)
        {
            var response = await _restIdealPostcodesApi.ExecuteTaskAsync<LocationServiceSingleResult<AddressLocationSingleResult>>(
                new RestRequest(_configuration.GetLocationByUDPRNUrl(udprn), Method.GET));
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result;
            throw response.ErrorException;
        }

        public async Task<LocationServiceResult<AddressLocationResult>> ValidateAndFindAddresses(string postcode)
        {
            var response = await _restIdealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<AddressLocationResult>>(
                new RestRequest(_configuration.GetLocationByPostcodeUrl(postcode), Method.GET));
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data;
            throw response.ErrorException;
        }
        public async Task<List<AddressLocationResult>> FindAddresses(string postcode)
        {
            var response = await _restIdealPostcodesApi.ExecuteTaskAsync<LocationServiceResult<AddressLocationResult>>(
                new RestRequest(_configuration.GetLocationByPostcodeUrl(postcode), Method.GET));
            if (response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result.ToList();
            throw response.ErrorException;
        }


    }
    public interface ILocationService
    {
        Task<List<PostcodeLocationResult>> FindPostcodes(double longitude, double latitude);
        Task<List<AddressLocationResult>> FindAddresses(double longitude, double latitude);
        Task<List<AddressLocationResult>> FindAddresses(string postcode);
        Task<AddressLocationSingleResult> FindAddressFromUDPRN(string udprn);
        Task<LocationServiceResult<AddressLocationResult>> ValidateAndFindAddresses(string postcode);
    }
}
