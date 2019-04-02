using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using NHS111.Business.Services;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class CareAdviceController : ApiController
    {
        private readonly ICareAdviceService _careAdviceService;
        private readonly ICacheManager<string, string> _cacheManager;
        public CareAdviceController(ICareAdviceService careAdviceService, ICacheManager<string, string> cacheManager)
        {
            _careAdviceService = careAdviceService;
            _cacheManager = cacheManager;
        }

        [HttpGet]
        [Route("pathways/care-advice/{age}/{gender}")]
        public async Task<JsonResult<IEnumerable<CareAdvice>>> GetCareAdvice(int age, string gender, [FromUri]string markers)
        {
#if !DEBUG
                var cacheKey = string.Format("CareAdvice-{0}-{1}-{2}", age, gender, markers);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<IEnumerable<CareAdvice>>(cacheValue));
                }
#endif

            markers = markers ?? string.Empty;
            var careAdvice = await _careAdviceService.GetCareAdvice(age, gender, markers.Split(','));
            
           #if !DEBUG  
                _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(careAdvice));
            #endif
            return Json(careAdvice);
        }

        [HttpPost]
        [Route("pathways/care-advice/{dxCode}/{ageCategory}/{gender}")]
        public async Task<JsonResult<IEnumerable<CareAdvice>>> GetCareAdvice(string dxCode, string ageCategory, string gender, [FromBody]string keywords)
        {
#if !DEBUG
                var cacheKey = string.Format("CareAdvice-{0}-{1}-{2}-{3}", dxCode, ageCategory, gender, keywords.Replace(' ', '_'));

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<IEnumerable<CareAdvice>>(cacheValue));
                }
#endif

            keywords = keywords ?? string.Empty;
            var careAdvice = await _careAdviceService.GetCareAdvice(ageCategory, gender, keywords, dxCode);
#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(careAdvice));
#endif
            return Json(careAdvice);
        }

       
    }
}