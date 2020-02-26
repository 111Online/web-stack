using System.Web.Mvc;
using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    public class DirectLinkingView<T>
        : WebViewPage<T>
    {
        protected readonly IDirectLinkingFeature DirectLinkingFeature;
        protected readonly IEmergencyAlertFeature EmergencyAlertFeature;

        public DirectLinkingView()
        {
            DirectLinkingFeature = new DirectLinkingFeature();
            EmergencyAlertFeature = new EmergencyAlertFeature();
        }

        public override void Execute() { }
    }
}