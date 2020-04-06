using NHS111.Web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    public class HealthzController : ApiController
    {
        [AllowAnonymousAccess]
        public string Get()
        {
            return "healthy";
        }
    }
}
