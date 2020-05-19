using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "orderNo")]
        public int OrderNo { get; set; }

        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }

    }
}
