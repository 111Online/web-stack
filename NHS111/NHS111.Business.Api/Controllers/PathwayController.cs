using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using NHS111.Business.Services;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class PathwayController : ApiController
    {
        private readonly ISearchCorrectionService _searchCorrectionService;
        private readonly IPathwayService _pathwayService;
        private readonly ICacheManager<string, string> _cacheManager;

        public PathwayController(IPathwayService pathwayService, ISearchCorrectionService searchCorrectionService, ICacheManager<string, string> cacheManager)
        {
            _searchCorrectionService = searchCorrectionService;
            _pathwayService = pathwayService;
            _cacheManager = cacheManager;
        }

        [Route("pathway/{id}")]
        public async Task<JsonResult<Pathway>> Get(string id)
        {
            var pathway = await _pathwayService.GetPathway(id);
            return Json(pathway);
        }

        [Route("pathway/metadata/{id}")]
        public async Task<JsonResult<PathwayMetaData>> GetMetaData(string id)
        {
            var pathway = await _pathwayService.GetPathwayMetaData(id);
            return Json(pathway);
        }

        [Route("pathway/{pathwayNumbers}/{gender}/{age}")]
        public async Task<JsonResult<Pathway>> GetByDetails(string pathwayNumbers, string gender, int age)
        {
            var pathway = await _pathwayService.GetIdentifiedPathway(pathwayNumbers, gender, age);
            return Json(pathway);
        }

        [Route("pathway/symptomGroup/{pathwayNumbers}/")]
        public async Task<JsonResult<string>> GetSymptomGroup(string pathwayNumbers)
        {
            var symptomGroup = await _pathwayService.GetSymptomGroup(pathwayNumbers);
            return Json(symptomGroup);
        }

        [Route("pathway")]
        public async Task<JsonResult<IEnumerable<Pathway>>> GetAll()
        {
            var pathways = await _pathwayService.GetPathways(false, false);
            return Json(pathways);
        }

        [Route("pathway/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<Pathway>>> GetAll(string gender, int age)
        {
            var cacheKey = String.Format("PathwayGetAll-{0}-{1}", gender, age);
#if !DEBUG
                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<IEnumerable<Pathway>>(cacheValue));
                }
#endif
            var pathways = await _pathwayService.GetPathways(false, false, gender, age);
#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(pathways));
#endif
            return Json(pathways);
        }

        [Route("pathway_suggest/{name}/{startingOnly}")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetSuggestedPathway(string name, bool startingOnly)
        {
            var suggestedPathways = await _searchCorrectionService.GetCorrection(name, startingOnly);
            return Json(suggestedPathways);
        }

        [Route("pathway_suggest/{name}/{startingOnly}/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetSuggestedPathway(string name, bool startingOnly, string gender, int age)
        {

            var cacheKey = String.Format("PathwayGetAllGrouped-{0}-{1}", gender, age);
#if !DEBUG
                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return Json(_searchCorrectionService.GetCorrection(JsonConvert.DeserializeObject<List<GroupedPathways>>(cacheValue), name));
                }
#endif
            var pathways = await _pathwayService.GetGroupedPathways(true, false);
#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(pathways));
#endif
            return Json(_searchCorrectionService.GetCorrection(pathways, name));
        }

        [Route("pathway_direct/{pathwayTitle}")]
        public async Task<JsonResult<string>> GetPathwayNumbers(string pathwayTitle)
        {
            var pathwayNumbers = await _pathwayService.GetPathwayNumbers(pathwayTitle);
            return Json(pathwayNumbers);
        }

        [Route("pathway_direct/identify/{pathwayTitle}/{gender}/{age}")]
        public async Task<JsonResult<Pathway>> GetPathwayDetails(string pathwayTitle, string gender, int age)
        {
            var pathway = await _pathwayService.GetIdentifiedPathwayFromTitle(pathwayTitle, gender, age);
            return Json(pathway);
        }

        [Route("pathway_question/{name}/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetPathwayQuestion(string name, bool startingOnly)
        {
            var pathways = await _searchCorrectionService.GetCorrection(name, startingOnly);
            return Json(pathways);
        }
    }
}