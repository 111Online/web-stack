using NHS111.Business.Services;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

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
            string cacheKey = String.Format("Pathway-{0}", id);

#if !DEBUG
            var cacheValue = await _cacheManager.Read(cacheKey);

            if (!String.IsNullOrEmpty(cacheValue))
                return Json(JsonConvert.DeserializeObject<Pathway>(cacheValue));
#endif

            var pathway = await _pathwayService.GetPathway(id);

#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(pathway));
#endif

            return Json(pathway);
        }

        [Route("pathway/metadata/{id}")]
        public async Task<JsonResult<PathwayMetaData>> GetMetaData(string id)
        {
            string cacheKey = String.Format("PathwayMetaData-{0}", id);

#if !DEBUG
            var cacheValue = await _cacheManager.Read(cacheKey);

            if (!String.IsNullOrEmpty(cacheValue))
                return Json(JsonConvert.DeserializeObject<PathwayMetaData>(cacheValue));
#endif

            var metadata = await _pathwayService.GetPathwayMetaData(id);

#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(metadata));
#endif

            return Json(metadata);
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
            return await GetGroupedPathways(name, startingOnly);
        }

        [Route("pathway_suggest/{name}/{startingOnly}/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<GroupedPathways>>> GetSuggestedPathway(string name, bool startingOnly, string gender, int age)
        {
            return await GetGroupedPathways(name, false);
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
            return await GetGroupedPathways(name, startingOnly);
        }

        private async Task<JsonResult<IEnumerable<GroupedPathways>>> GetGroupedPathways(string name, bool startingOnly)
        {
            var cacheKey = String.Format("PathwaysGetGrouped-{0}-{1}", name, startingOnly);

#if !DEBUG
            var cacheValue = await _cacheManager.Read(cacheKey);

            if (!string.IsNullOrEmpty(cacheValue))
                return Json(JsonConvert.DeserializeObject<IEnumerable<GroupedPathways>>(cacheValue));
#endif

            var pathways = await _pathwayService.GetGroupedPathways(true, startingOnly);
            var corrected = _searchCorrectionService.GetCorrection(pathways, name);

#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(corrected));
#endif

            return Json(corrected);
        }
    }
}