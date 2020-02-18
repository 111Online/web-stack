using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHS111.Features;

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