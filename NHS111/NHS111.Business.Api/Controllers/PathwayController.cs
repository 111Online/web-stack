using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.Services;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class PathwayController : ApiController
    {
        private readonly ISearchCorrectionService _searchCorrectionService;
        private readonly IPathwayService _pathwayService;

        public PathwayController(IPathwayService pathwayService, ISearchCorrectionService searchCorrectionService)
        {
            _searchCorrectionService = searchCorrectionService;
            _pathwayService = pathwayService;
        }

        [Route("pathway/{id}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            return await _pathwayService.GetPathway(id).AsHttpResponse();
        }

        [Route("pathway/{pathwayNumbers}/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetByDetails(string pathwayNumbers, string gender, int age)
        {
            return await _pathwayService.GetIdentifiedPathway(pathwayNumbers, gender, age).AsHttpResponse();
        }

        [Route("pathway/symptomGroup/{pathwayNumbers}/")]
        public async Task<HttpResponseMessage> GetSymptomGroup(string pathwayNumbers)
        {
            return await _pathwayService.GetSymptomGroup(pathwayNumbers).AsHttpResponse();
        }

        [Route("pathway")]
        public async Task<HttpResponseMessage> GetAll()
        {
            return await _pathwayService.GetPathways(false).AsHttpResponse();
        }

        [Route("pathway_suggest/{name}")]
        public async Task<HttpResponseMessage> GetSuggestedPathway(string name)
        {
            return await _searchCorrectionService.GetCorrection(name).AsHttpResponse();
        }

        [Route("pathway_direct/{pathwayTitle}")]
        public async Task<HttpResponseMessage> GetPathwayNumbers(string pathwayTitle)
        {
            return await _pathwayService.GetPathwayNumbers(pathwayTitle).AsHttpResponse();
        }

        [Route("pathway_direct/identify/{pathwayTitle}/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetPathwayDetails(string pathwayTitle, string gender, int age)
        {
            return await _pathwayService.GetIdentifiedPathwayFromTitle(pathwayTitle, gender, age).AsHttpResponse();
        }

        [Route("pathway_question/{name}/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetPathwayQuestion(string name)
        {
            return await _searchCorrectionService.GetCorrection(name).AsHttpResponse();
        }
    }
}