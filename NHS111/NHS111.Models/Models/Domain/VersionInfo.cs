using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class VersionInfo
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "build")]
        public string Build { get; set; }

        [JsonProperty(PropertyName = "dateTime")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "pathwaysBranch")]
        public string ContentBranch { get; set; }

        [JsonProperty(PropertyName = "dataBranch")]
        public string DataBranch { get; set; }
    }
}
