using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
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
        public async Task<JsonResult<IEnumerable<Pathway>>> GetPathways([FromUri]bool startingOnly = false)
        {
            var pathways = await _pathwayRepository.GetAllPathways(startingOnly);
            return Json(pathways);
        }

        [HttpGet]
        [Route("groupedpathways")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetGroupedPathways([FromUri]bool startingOnly = false)
        {
            var pathways = await _pathwayRepository.GetGroupedPathways(startingOnly);
            return Json(pathways);
        }

        [HttpGet]
        [Route("pathways/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<Pathway>>> GetPathways(string gender, int age, [FromUri]bool startingOnly = false)
        {
            var pathways = await _pathwayRepository.GetAllPathways(startingOnly, gender, age);
            return Json(pathways);
        }

        [HttpGet]
        [Route("groupedpathways/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetGroupedPathways(string gender, int age, [FromUri]bool startingOnly = false)
        {
            var pathways = await _pathwayRepository.GetGroupedPathways(startingOnly, gender, age);
            return Json(pathways);
        }

        [HttpGet]
        [Route("pathways/{pathwayId}")]
        public async Task<JsonResult<Pathway>> GetPathway(string pathwayId)
        {
            var pathway = await _pathwayRepository.GetPathway(pathwayId);
            return Json(pathway);
        }

        [HttpGet]
        [Route("pathways/metadata/{pathwayId}")]
        public async Task<JsonResult<PathwayMetaData>> GetPathwayMetadata(string pathwayId)
        {
            var pathway = await _pathwayRepository.GetPathwayMetadata(pathwayId);
            return Json(pathway);
        }

        [HttpGet]
        [Route("pathways/identify/{pathwayNumbers}")]
        public async Task<JsonResult<Pathway>> GetIdentifiedPathway(string pathwayNumbers, [FromUri]string gender, [FromUri]int age)
        {
            var pathway = await _pathwayRepository.GetIdentifiedPathway(pathwayNumbers.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), gender, age);
            return Json(pathway);
        }

        [HttpGet]
        [Route("pathways/symptomGroup/{pathwayNumbers}")]
        public async Task<JsonResult<string>> GetSymptomGroup(string pathwayNumbers)
        {
            var symptomGroup = await _pathwayRepository.GetSymptomGroup(pathwayNumbers.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            return Json(symptomGroup);
        }

        [HttpGet]
        [Route("pathways_direct/{pathwayTitle}")]
        public async Task<JsonResult<string>> GetPathwaysNumbers(string pathwayTitle)
        {
            var pathwayNumbers = await _pathwayRepository.GetPathwaysNumbers(pathwayTitle);
            return Json(pathwayNumbers);
        }

        [HttpGet]
        [Route("pathways_direct/identify/{pathwayTitle}")]
        public async Task<JsonResult<Pathway>> GetIdentifiedPathwayFromTitle(string pathwayTitle, [FromUri]string gender, [FromUri]int age)
        {
            var pathway = await _pathwayRepository.GetIdentifiedPathway(pathwayTitle, gender, age);
            return Json(pathway);
        }
    }
}