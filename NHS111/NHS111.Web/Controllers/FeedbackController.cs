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
        public async Task<JsonResult> SubmitFeedback(FeedbackViewModel feedbackData)
        {
            feedbackData.DateAdded = DateTime.Now; //this is when the feedback is added
            return Json(await _feedbackViewModelBuilder.FeedbackBuilder(feedbackData));
        }
    }
}