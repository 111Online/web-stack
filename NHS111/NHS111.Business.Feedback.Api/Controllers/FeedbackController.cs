using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NHS111.Utils.Attributes;

namespace NHS111.Business.Feedback.Api.Controllers
{
    [LogHandleErrorForApi]
    public class FeedbackController : ApiController
    {
        public FeedbackController()
        {
            
        }
    }
}
