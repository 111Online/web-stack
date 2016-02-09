using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Domain.Repository;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Api.Controllers
{
    [LogHandleErrorForApi]
    public class PathwayController : ApiController
    {
        private readonly IPathwayRepository _pathwayRepository;

        public PathwayController(IPathwayRepository pathwayRepository)
        {
            _pathwayRepository = pathwayRepository;
        }

        [HttpGet]
        [Route("pathways")]
        public async Task<HttpResponseMessage> GetPathways([FromUri]bool grouped = false)
        {
            return grouped
                ? await _pathwayRepository.GetGroupedPathways().AsJson().AsHttpResponse()
                : await _pathwayRepository.GetAllPathways().AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/{pathwayId}")]
        public async Task<HttpResponseMessage> GetPathway(string pathwayId)
        {
            return await _pathwayRepository.GetPathway(pathwayId).AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/identify/{pathwayNumbers}")]
        public async Task<HttpResponseMessage> GetIdentifiedPathway(string pathwayNumbers, [FromUri]string gender, [FromUri]int age)
        {
            return await _pathwayRepository.GetIdentifiedPathway(pathwayNumbers.Split(new []{","},StringSplitOptions.RemoveEmptyEntries), gender, age).AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/symptomGroup/{pathwayNumbers}")]
        public async Task<HttpResponseMessage> GetSymptomGroup(string pathwayNumbers)
        {
            return await _pathwayRepository.GetSymptomGroup(pathwayNumbers.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries)).AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways_direct/{pathwayTitle}")]
        public async Task<HttpResponseMessage> GetPathwaysNumbers(string pathwayTitle)
        {
            return await _pathwayRepository.GetPathwaysNumbers(pathwayTitle).AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways_direct/identify/{pathwayTitle}")]
        public async Task<HttpResponseMessage> GetIdentifiedPathwayFromTitle(string pathwayTitle, [FromUri]string gender, [FromUri]int age)
        {
            return await _pathwayRepository.GetIdentifiedPathway(pathwayTitle, gender, age).AsJson().AsHttpResponse();
        }
    }
}