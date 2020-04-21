using NHS111.Features;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{
    public class SearchView<T> : WebViewPage<T>
    {
        protected readonly IEmergencyAlertFeature EmergencyAlertFeature;

        public SearchView()
        {
            EmergencyAlertFeature = new EmergencyAlertFeature();
        }

        public override void Execute() { }
    }
}