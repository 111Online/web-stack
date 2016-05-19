using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Domain.Repository;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Api.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using Models.Models.Domain;

    [LogHandleErrorForApi]
    public class CareAdviceController : ApiController
    {
        private readonly ICareAdviceRepository _careAdviceRepository;

        public CareAdviceController(ICareAdviceRepository careAdviceRepository)
        {
            _careAdviceRepository = careAdviceRepository;
        }

        [HttpGet]
        [Route("pathways/care-advice/{age}/{gender}")]
        public async Task<HttpResponseMessage> GetCareAdvice(int age, string gender, [FromUri]string markers)
        {
            markers = markers ?? string.Empty;
            return await _careAdviceRepository.GetCareAdvice(age, gender, markers.Split(',')).AsJson().AsHttpResponse();
        }

        [HttpPost]
        [Route("pathways/care-advice/{dxCode}/{ageCategory}/{gender}")]
        public async Task<HttpResponseMessage> GetCareAdvice(string dxCode, string ageCategory, string gender, [FromBody]string keywords)
        {
            keywords = keywords ?? string.Empty;
            return await _careAdviceRepository.GetCareAdvice(new AgeCategory(ageCategory), new Gender(gender), keywords.Split('|'), new DispositionCode(dxCode)).AsJson().AsHttpResponse();
        }
    }
}