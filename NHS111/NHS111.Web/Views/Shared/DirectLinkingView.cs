using NHS111.Features;
using System.Web.Mvc;

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