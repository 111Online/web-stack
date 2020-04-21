
using NHS111.Features;
using NHS111.Models.Models.Web;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{

    public class DisclaimerPopupView
        : WebViewPage<JourneyViewModel>
    {

        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }

        public DisclaimerPopupView()
        {
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
        }

        public override void Execute() { }
    }
}