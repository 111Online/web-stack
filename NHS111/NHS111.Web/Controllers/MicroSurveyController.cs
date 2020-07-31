using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using NHS111.Models.Models.Web.MicroSurvey;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{
    public class MicroSurveyController : Controller
    {
        private readonly IMicroSurveyBuilder _microSurveyBuilder;

        public MicroSurveyController(IMicroSurveyBuilder microSurveyBuilder)
        {
            _microSurveyBuilder = microSurveyBuilder;
        }

        [System.Web.Mvc.HttpPost]
        public async Task PostRecommendedServiceSurvey([FromBody] SurveyResult surveyResult)
        {
            await _microSurveyBuilder.PostMicroSurveyResponse(surveyResult);
        }
    }
}