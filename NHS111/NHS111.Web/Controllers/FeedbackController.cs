using System.Threading.Tasks;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
  public class FeedbackController : Controller
  {
    // GET: Feedback
    [HttpPost]
    public async Task<ActionResult> SubmitFeedback(Models.Models.Web.Feedback feedbackData)
    {
      return View("../Shared/FeedbackConfirmation", await _feedbackBuilder.SubmitDataBuilder(feedbackData));
    }
  }
}