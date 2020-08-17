using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Outcome;

namespace NHS111.Models.Models.Web
{
    public class RecommendedServiceViewModel : ServiceViewModel
    {
        public string ReasonText { get; set; }
        public DetailsViewModel Details { get; set; }        
    }
}
