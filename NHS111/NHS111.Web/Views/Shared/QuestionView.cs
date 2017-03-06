using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    using System.Web.Mvc;

    public class QuestionView<T>
        : WebViewPage<T>
    {
        protected readonly IDirectLinkingFeature DirectLinkingFeature;
        protected readonly IUserZoomSurveyFeature UserZoomSurveyFeature;

        public QuestionView()
        {
            DirectLinkingFeature = new DirectLinkingFeature();
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
        }

        public override void Execute() { }
    }
}