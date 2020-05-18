
namespace NHS111.Models.Models.Domain
{
    using Newtonsoft.Json;

    public class SymptomDiscriminator
    {

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "reasoningText")]
        public string ReasoningText { get; set; }
    }
}
