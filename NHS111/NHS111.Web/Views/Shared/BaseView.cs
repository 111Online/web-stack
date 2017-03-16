using NHS111.Features;

namespace NHS111.Web.Views.Shared {
    using System.Web.Mvc;

    public class BaseView<T>
        : WebViewPage<T> {

        public IUserZoomSurveyFeature UserZoomSurveyFeature { get; set; }

        public BaseView()
        {
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
        }

        public override void Execute() { }
    }
}