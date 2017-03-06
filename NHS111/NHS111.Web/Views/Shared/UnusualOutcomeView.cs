using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    using System.Web.Mvc;

    public class UnusualOutcomeView<T>
        : WebViewPage<T>
    {
        protected readonly ISurveyLinkFeature SurveyLinkFeature;
        protected readonly IUserZoomSurveyFeature UserZoomSurveyFeature;

        public UnusualOutcomeView()
        {
            SurveyLinkFeature = new SurveyLinkFeature();
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
        }

        public override void Execute() { }
    }
}