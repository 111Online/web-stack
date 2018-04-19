using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Business.Location
{
    public class LocationServiceResult<T>
    {

        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }
    

    
    }

    public class PostcodeLocationResult
    {
        [JsonProperty(PropertyName = "postcode")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "northings")]
        public int Northings { get; set; }

        [JsonProperty(PropertyName = "eastings")]
        public int Eastings { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }
    }

    public class AddressLocationResult
    {

        [JsonProperty(PropertyName = "udprn")]
        public string Udprn { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "northings")]
        public int Northings { get; set; }

        [JsonProperty(PropertyName = "eastings")]
        public int Eastings { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "post_town")]
        public string Town { get; set; }

        [JsonProperty(PropertyName = "thoroughfare")]
        public string Thoroughfare { get; set; }

        [JsonProperty(PropertyName = "building_number")]
        public string BuildingNumber { get; set; }

        [JsonProperty(PropertyName = "building_name")]
        public string BuildingName { get; set; }

        [JsonProperty(PropertyName = "sub_building_name")]
        public string SubBuildingName { get; set; }

        [JsonProperty(PropertyName = "line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "line_2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "line_3")]
        public string AddressLine3 { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "ward")]
        public string Ward { get; set; }



    }
}
