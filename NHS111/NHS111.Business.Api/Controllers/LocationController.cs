using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using NHS111.Business.Services;
using NHS111.Models.Models.Business.Location;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class LocationController : ApiController
    {
        private readonly ILocationService _locatioService;

        public LocationController(ILocationService locationService)
        {
            _locatioService = locationService;
        }

        [Route("postcode/geo/{longlat}")]
        [HttpGet]
        public async Task<JsonResult<List<PostcodeLocationResult>>> GetPostcode(string longlat)
        {
            var geolocation = GetGeoLocationParams(longlat);
            var results = await _locatioService.FindPostcodes(geolocation.Item1, geolocation.Item2);
            return Json(results);
        }

        [Route("address/postcode/{postcode}")]
        [HttpGet]
        public async Task<JsonResult<List<AddressLocationResult>>> GetLocation(string postcode)
        {
         
            var results = await _locatioService.FindAddresses(postcode);
            return Json(results);
        }

        [Route("address/geo/{longlat}")]
        [HttpGet]
        public async Task<JsonResult<List<AddressLocationResult>>> GetLocationByGeo(string longlat)
        {
            var geolocation = GetGeoLocationParams(longlat);
            var results = await _locatioService.FindAddresses(geolocation.Item1,geolocation.Item2);
            return Json(results);
        }

        private Tuple<double, double> GetGeoLocationParams(string longlat)
        {
            var longlatArray = ParselonglatParam(longlat);
            var geolocation = ParselongLatArray(longlatArray);
            return geolocation;
        }


        private Tuple<double, double> ParselongLatArray(string[] longlatParams)
        {
            if (longlatParams.Length != 2)
                throw new ArgumentOutOfRangeException("Incorrect number of paramaters passed. Format should be {long},{lat}");

            double longparam = Double.NaN;
            double latparam = Double.NaN; ;
            try
            {
                 longparam = Convert.ToDouble(longlatParams[0]);
                 latparam = Convert.ToDouble(longlatParams[1]);
              
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException(
                    "both lat and long params must be a decimal number. Format should be {long},{lat}", ex);
            }
            return new Tuple<double, double>(longparam, latparam);
          
        }

        private string[] ParselonglatParam(string longlat)
        {
            try
            {
                return longlat.Split(',');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("longlat argument incorrectly formatted. Format should be {long},{lat}",
                    ex);
            }
        }
    }
}