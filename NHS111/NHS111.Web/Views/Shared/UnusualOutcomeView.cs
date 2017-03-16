using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    using System.Web.Mvc;

    public class UnusualOutcomeView<T>
        : BaseView<T>
    {
        protected readonly ISurveyLinkFeature SurveyLinkFeature;

        public UnusualOutcomeView()
        {
            SurveyLinkFeature = new SurveyLinkFeature();
        }

        public override void Execute() { }
    }
}