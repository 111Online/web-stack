using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Tools.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackViewModelBuilder _feedbackViewModelBuilder;
        private readonly IConfiguration _configuration;

        public FeedbackController(IFeedbackViewModelBuilder feedbackViewModelBuilder, IConfiguration configuration)
        {
            _feedbackViewModelBuilder = feedbackViewModelBuilder;
            _configuration = configuration;
        }

        public async Task<ActionResult> Home()
        {
            var model = await _feedbackViewModelBuilder.ViewFeedbackBuilder();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(FeedbackViewModel feedback)
        {
            var url = string.Format(_configuration.FeedbackDeleteFeedbackUrl, feedback.PartitionKey, feedback.RowKey);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, new Uri(url))
            {
                Version = HttpVersion.Version10, //forcing 1.0 to prevent Expect 100 Continue header
                Headers = { { "Authorization", _configuration.FeedbackAuthorization } }
            };

            using (var http = new HttpClient())
            {
                try
                {
                    await http.SendAsync(httpRequestMessage);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("An error occured requesting {0}. See inner exception for details.", url), e);
                }
            }
           
            ModelState.Clear();
            var model = await _feedbackViewModelBuilder.ViewFeedbackBuilder();
            return PartialView("_FeedbackList", model.ToList());
        }
    }
}