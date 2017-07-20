using System.Web.Mvc;
using NHS111.Features;

namespace NHS111.Web.Tools.Views.Shared
{
    public class FeedbackRowView<T> : WebViewPage<T>
    {
        protected readonly IDeleteFeedbackFeature DeleteFeedbackFeature;

        public FeedbackRowView()
        {
            DeleteFeedbackFeature = new DeleteFeedbackFeature();
        }

        public override void Execute() { }
    }
}