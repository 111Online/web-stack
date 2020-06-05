using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Builders;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFeedbackViewModelBuilder _feedbackViewModelBuilder;

        public FeedbackController(IFeedbackViewModelBuilder feedbackViewModelBuilder, IConfiguration configuration)
        {
            _feedbackViewModelBuilder = feedbackViewModelBuilder;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> SubmitFeedback(FeedbackViewModel feedbackData)
        {
            if (!_configuration.FeedbackEnabled)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "Feedback is disabled and not allowed.");
            }
            
            var model = await _feedbackViewModelBuilder.FeedbackResultBuilder(feedbackData);
            return Content(model.Message, "text/html");
        }
    }
}
