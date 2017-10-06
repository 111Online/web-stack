using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.Location;
using RestSharp;

namespace NHS111.Business.Services
{
    public class LocationService: ILocationService
    {
        private readonly IRestClient _restidealPostcodesApi;
        private readonly IConfiguration _configuration;

        public LocationService(IRestClient restidealPostcodesApi, IConfiguration configuration)
        {
            _restidealPostcodesApi = restidealPostcodesApi;
            _configuration = configuration;
        }

        public async Task<List<GeoLocationResult>> FindPostcodes(double longitude, double latitude)
        {
            var response = await _restidealPostcodesApi.ExecuteTaskAsync<PostcodeResult>(
                new RestRequest(_configuration.GetLocationPostcodebyGeoUrl(longitude, latitude), Method.GET));

            if(response.ResponseStatus == ResponseStatus.Completed)
                return response.Data.Result.ToList();
            throw response.ErrorException;
        }
    }
     public interface ILocationService
    {
         Task<List<GeoLocationResult>> FindPostcodes(double longitude, double latitude);
    }
}
