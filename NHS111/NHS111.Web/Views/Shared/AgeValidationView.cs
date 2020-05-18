using NHS111.Features;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{
    public class AgeValidationView<T> : WebViewPage<T>
    {
        protected readonly IFilterPathwaysByAgeFeature FilterPathwaysByAgeFeature;
        protected readonly IEmergencyAlertFeature EmergencyAlertFeature;

        public AgeValidationView()
        {
            FilterPathwaysByAgeFeature = new FilterPathwaysByAgeFeature();
            EmergencyAlertFeature = new EmergencyAlertFeature();
        }

        public override void Execute() { }
    }
}