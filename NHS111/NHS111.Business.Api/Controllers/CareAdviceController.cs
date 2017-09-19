using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using NHS111.Business.Services;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> GetCareAdvice(int age, string gender, [FromUri]string markers)
        {
            #if !DEBUG
                var cacheKey = string.Format("CareAdvice-{0}-{1}-{2}", age, gender, markers);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
            #endif

            markers = markers ?? string.Empty;
            var response = await _careAdviceService.GetCareAdvice(age, gender, markers.Split(',')).AsHttpResponse();
          
            #if !DEBUG  
                _cacheManager.Set(cacheKey, response.Content.ReadAsStringAsync().Result);
            #endif
            return response;
        }

        [HttpPost]
        [Route("pathways/care-advice/{dxCode}/{ageCategory}/{gender}")]
        public async Task<HttpResponseMessage> GetCareAdvice(string dxCode, string ageCategory, string gender, [FromBody]string keywords)
        {
            #if !DEBUG
                var cacheKey = string.Format("CareAdvice-{0}-{1}-{2}-{3}", dxCode, ageCategory, gender, keywords.Replace(' ', '_'));

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
            #endif

            keywords = keywords ?? string.Empty;
            var response = await _careAdviceService.GetCareAdvice(ageCategory, gender, keywords, dxCode).AsHttpResponse();
            #if !DEBUG  
            _cacheManager.Set(cacheKey, response.Content.ReadAsStringAsync().Result);
            #endif
            return response;
        }

       
    }
}