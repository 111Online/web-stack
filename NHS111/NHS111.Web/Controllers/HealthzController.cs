using NHS111.Web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NHS111.Web.Controllers
{
    public class HealthzController : ApiController
    {
        /// <summary>
        /// This health endpoint is used by FrontDoor
        /// </summary>
        /// <returns></returns>
        [AllowAnonymousAccess]
        [HttpGet]
        [HttpHead]
        public string Get()
        {
            return "healthy";
        }
    }
}
