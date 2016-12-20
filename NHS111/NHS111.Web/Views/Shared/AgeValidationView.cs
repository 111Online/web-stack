using System;
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

        public AgeValidationView()
        {
            FilterPathwaysByAgeFeature = new FilterPathwaysByAgeFeature();
        }

        public override void Execute() { }
    }
}