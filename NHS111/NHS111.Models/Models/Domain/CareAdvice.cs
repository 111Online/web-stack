using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class CareAdvice
    {
        public CareAdvice() { }

        public CareAdvice(List<CareAdviceText> items)
        {
            this.Items = items;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "keyword")]
        public string Keyword { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<CareAdviceText> Items { get; set; }
    }
}