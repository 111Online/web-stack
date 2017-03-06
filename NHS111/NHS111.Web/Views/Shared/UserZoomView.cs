using System.Web.Mvc;
using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    public class UserZoomView<T> : WebViewPage<T>
    {
        public IUserZoomSurveyFeature UserZoomSurveyFeature { get; set; }

        public UserZoomView()
        {
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
        }

        public override void Execute() { }
    }
}