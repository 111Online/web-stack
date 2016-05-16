using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class CareAdvice
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "excludeKeywords")]
        public IEnumerable<string> ExcludeKeywords { get; set; }

        [JsonProperty(PropertyName = "items")]
        public IEnumerable<string> Items { get; set; }
    }
}