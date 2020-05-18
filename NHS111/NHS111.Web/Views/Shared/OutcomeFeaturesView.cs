using NHS111.Features;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{
    public class OutcomeFeaturesView<T> : WebViewPage<T>
    {
        public ISurveyLinkFeature SurveyLinkFeature { get; set; }
        public IDirectLinkingFeature DirectLinkingFeature { get; set; }

        public OutcomeFeaturesView()
        {
            SurveyLinkFeature = new SurveyLinkFeature();
            DirectLinkingFeature = new DirectLinkingFeature();
        }

        public override void Execute() { }
    }
}