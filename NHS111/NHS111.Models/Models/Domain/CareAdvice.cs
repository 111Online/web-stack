using Newtonsoft.Json;
using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    using System.Diagnostics;

    [DebuggerDisplay("{Id}: {Keyword} ({Items.Count})")]
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
