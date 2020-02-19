using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackViewModelBuilder _feedbackViewModelBuilder;

        public FeedbackController(IFeedbackViewModelBuilder feedbackViewModelBuilder)
        {
            _feedbackViewModelBuilder = feedbackViewModelBuilder;
        }

        [HttpPost]
        public async Task<ActionResult> SubmitFeedback(FeedbackViewModel feedbackData)
        {
            var model = await _feedbackViewModelBuilder.FeedbackResultBuilder(feedbackData);
            return Content(model.Message, "text/html");
        }
    }
}