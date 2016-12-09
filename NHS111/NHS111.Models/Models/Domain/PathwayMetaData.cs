using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class PathwayMetaData
    {
        [JsonProperty(PropertyName = "pathwayNo")]
        public string PathwayNo { get; set; }

        [JsonProperty(PropertyName = "digitalDescriptionId")]
        public string DigitalTitleId { get; set; }

        [JsonProperty(PropertyName = "digitalDescription")]
        public string DigitalTitle { get; set; }
    }
}
