using Newtonsoft.Json;

namespace NHS111.Models.Models.Web
{
  public class FeedbackConfirmation
  {
        private static string _successMessage = @"<p>Thank you.<br />If you want to give more detailed feedback, there’s a survey after the symptom questions.</p>";
        private static string _errorMessage = "Sorry, there is a technical problem. Try again in a few moments.";

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "wasSuccessful")]
        public bool WasSuccessful { get; set; }

        public static FeedbackConfirmation Success = new FeedbackConfirmation { Message = _successMessage, WasSuccessful = true };
        public static FeedbackConfirmation Error = new FeedbackConfirmation { Message = _errorMessage, WasSuccessful = false };

    }
}