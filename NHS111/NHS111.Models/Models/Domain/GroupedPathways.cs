using Newtonsoft.Json;
using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    public class GroupedPathways
    {
        [JsonProperty(PropertyName = "pathwayNumbers")]
        public IEnumerable<string> PathwayNumbers { get; set; }

        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }
    }
}