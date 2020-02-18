﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHS111.Features;

namespace NHS111.Web.Views.Shared
{
    public class AgeValidationView<T>: WebViewPage<T>
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