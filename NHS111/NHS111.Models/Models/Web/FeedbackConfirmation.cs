using Newtonsoft.Json;

namespace NHS111.Models.Models.Web
{
  public class FeedbackConfirmation
  {
        private static string _successMessage = @"<p>Thank you.</p><p>We cannot reply to any comments or pass them on to other NHS services.</p><p>To get medical help you should carry on answering the questions or call 111.</p> <p>If you'd like to give more feedback about using 111 online, there's a survey at the end of the questions.</p>";
        private static string _errorMessage = @"<p>Sorry, there is a technical problem. Try again in a few moments.</p>";

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "wasSuccessful")]
        public bool WasSuccessful { get; set; }

        public static FeedbackConfirmation Success = new FeedbackConfirmation { Message = _successMessage, WasSuccessful = true };
        public static FeedbackConfirmation Error = new FeedbackConfirmation { Message = _errorMessage, WasSuccessful = false };

    }
}