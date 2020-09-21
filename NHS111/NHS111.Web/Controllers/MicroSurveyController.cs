using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.MicroSurvey;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.MicroSurvey;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class MicroSurveyController : Controller
    {
        private readonly IMicroSurveyBuilder _microSurveyBuilder;
        private readonly IAuditLogger _auditLogger;

        public MicroSurveyController(IMicroSurveyBuilder microSurveyBuilder, IAuditLogger auditLogger)
        {
            _microSurveyBuilder = microSurveyBuilder;
            _auditLogger = auditLogger;
        }

        [System.Web.Mvc.HttpPost]
        public async Task PostRecommendedServiceSurvey([FromBody] SurveyResult surveyResult)
        {
            var response = await _microSurveyBuilder.PostMicroSurveyResponse(surveyResult).ConfigureAwait(false);
            if (!response.IsSuccessful)
                throw new HttpException(string.Format("There was a problem requesting {0}. {1}", response.Request.Resource, response.ErrorMessage));
        }
    }
}