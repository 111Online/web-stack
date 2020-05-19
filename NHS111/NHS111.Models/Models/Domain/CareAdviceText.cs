using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Models.Models.Domain
{
    public class CareAdviceText
    {
        public CareAdviceText() { }
        public CareAdviceText(IEnumerable<CareAdviceText> items)
        {
            this.Items = items.ToList();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "orderNo")]
        public int OrderNo { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        public List<CareAdviceText> Items { get; set; }
    }

    public class CareAdviceTextWithParent : CareAdviceText
    {
        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }
    }
}
