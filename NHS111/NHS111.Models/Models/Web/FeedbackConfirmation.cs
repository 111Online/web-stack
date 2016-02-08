using Newtonsoft.Json;

namespace NHS111.Models.Models.Web
{
  public class FeedbackConfirmation
  {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
  }
}