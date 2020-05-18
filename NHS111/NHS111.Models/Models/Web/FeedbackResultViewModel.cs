namespace NHS111.Models.Models.Web
{
    public abstract class FeedbackResultViewModel
    {
        public FeedbackViewModel Feedback;

        protected FeedbackResultViewModel(FeedbackViewModel feedback)
        {
            Feedback = feedback;
        }

        protected string ResolveMessageByPathway(bool displayShortenedMessage)
        {
            return displayShortenedMessage ?
                @"<p>Thanks for your help in improving this service.</p><p>We cannot pass your comments to any other part of the NHS.</p><p>Call 111 if you need medical help.</p>" :
                @"<p>Thanks for your help in improving this service.</p><p>We cannot pass your comments to any other part of the NHS.</p><p>Call 111 if you need medical help.</p> <p>If you'd like to give more feedback about using 111 online, there's a survey at the end of the questions.</p>"; ;
        }

        public abstract string Message { get; }
        public bool WasSuccessful { get; set; }
    }

    public class FeedbackConfirmationResultViewModel : FeedbackResultViewModel
    {
        public FeedbackConfirmationResultViewModel(FeedbackViewModel feedback) : base(feedback)
        {
            WasSuccessful = true;
        }

        public override string Message { get { return ResolveMessageByPathway(this.Feedback.PageData.IsCoronaJourney || Feedback.PageData.IsSmsJourney); } }

    }

    public class FeedbackErrorResultViewModel : FeedbackResultViewModel
    {
        public FeedbackErrorResultViewModel(FeedbackViewModel feedback) : base(feedback)
        {
            WasSuccessful = false;
        }

        public override string Message { get { return @"<p>Sorry, there is a technical problem. Try again in a few moments.</p>"; } }
    }
}
