using System.Threading.Tasks;
using System.Web.Mvc;
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
        public async Task<ActionResult> SubmitFeedback(Models.Models.Domain.Feedback feedbackData)
        {
            return View("../Shared/FeedbackConfirmation", await _feedbackViewModelBuilder.FeedbackBuilder(feedbackData));
        }
    }
}