using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.Services;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> GetCareAdvice(int age, string gender, [FromUri]string markers)
        {
            markers = markers ?? string.Empty;
            return await _careAdviceService.GetCareAdvice(age, gender, markers.Split(',')).AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/care-advice/{dxCode}/{ageCategory}/{gender}")]
        public async Task<HttpResponseMessage> GetCareAdvice(string dxCode, string ageCategory, string gender, [FromBody]string keywords)
        {
            keywords = keywords ?? string.Empty;
            return await _careAdviceService.GetCareAdvice(ageCategory, gender, keywords.Split('|'), dxCode).AsHttpResponse();
        }
    }
}