using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Business.Location
{
    public class PostcodeResult
    {
        [JsonProperty(PropertyName = "code")]
        public string ResponseCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "result")]
        public IEnumerable<GeoLocationResult> Result { get; set; }
    

    
    }

    public class GeoLocationResult
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
}
