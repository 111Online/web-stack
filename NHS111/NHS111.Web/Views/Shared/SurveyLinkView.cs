using NHS111.Features;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{
    public class SurveyLinkView<T> : WebViewPage<T>
    {
        public ISurveyLinkFeature SurveyLinkFeature { get; set; }

        public SurveyLinkView()
        {
            SurveyLinkFeature = new SurveyLinkFeature();
        }

        public override void Execute() { }
    }
}