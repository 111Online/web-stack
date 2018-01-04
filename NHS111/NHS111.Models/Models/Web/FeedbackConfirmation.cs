using Newtonsoft.Json;

namespace NHS111.Models.Models.Web
{
  public class FeedbackConfirmation
  {
        private static string _successMessage = @"Thank you.<br>
                                                  We use feedback to improve the service, but can't reply to any comments.<br>
                                                  There’s a survey after the symptom questions, where you can give more detailed feedback if you like.";
        private static string _errorMessage = "Sorry, there is a technical problem. Try again in a few moments.";

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "wasSuccessful")]
        public bool WasSuccessful { get; set; }

        public static FeedbackConfirmation Success = new FeedbackConfirmation { Message = _successMessage, WasSuccessful = true };
        public static FeedbackConfirmation Error = new FeedbackConfirmation { Message = _errorMessage, WasSuccessful = false };

    }
}