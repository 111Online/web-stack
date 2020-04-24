﻿using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Builders;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            var model = await _feedbackViewModelBuilder.FeedbackResultBuilder(feedbackData).ConfigureAwait(false);
            return Content(model.Message, "text/html");
        }
    }
}