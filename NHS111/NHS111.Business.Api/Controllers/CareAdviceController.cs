using NHS111.Business.Services;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class CareAdviceController : ApiController
    {
        private readonly ICareAdviceService _careAdviceService;
        public CareAdviceController(ICareAdviceService careAdviceService)
        {
            _careAdviceService = careAdviceService;
        }

        [HttpGet]
        [Route("pathways/care-advice/{age}/{gender}")]
        public async Task<JsonResult<IEnumerable<CareAdvice>>> GetCareAdvice(int age, string gender, [FromUri]string markers)
        {
            markers = markers ?? string.Empty;
            var careAdvice = await _careAdviceService.GetCareAdvice(age, gender, markers.Split(','));

            return Json(careAdvice);
        }

        [HttpPost]
        [Route("pathways/care-advice/{dxCode}/{ageCategory}/{gender}")]
        public async Task<JsonResult<IEnumerable<CareAdvice>>> GetCareAdvice(string dxCode, string ageCategory, string gender, [FromBody]string keywords)
        {
            keywords = keywords ?? string.Empty;
            var careAdvice = await _careAdviceService.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            return Json(careAdvice);
        }
    }
}