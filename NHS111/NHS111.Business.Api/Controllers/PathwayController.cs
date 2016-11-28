using System;
using System.Collections.Generic;
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
            return await _pathwayService.GetPathways(false, false).AsHttpResponse();
        }

        [Route("pathway/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetAll(string gender, int age)
        {
            var cacheKey = String.Format("PathwayGetAll-{0}-{1}", gender, age);
              #if !DEBUG
                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
            #endif

            var result = await _pathwayService.GetPathways(false, false, gender, age);
            #if !DEBUG
            _cacheManager.Set(cacheKey, result);
            #endif
            return result.AsHttpResponse();
        }

        [Route("pathway_suggest/{name}/{startingOnly}")]
        public async Task<HttpResponseMessage> GetSuggestedPathway(string name, bool startingOnly)
        {
            return await _searchCorrectionService.GetCorrection(name, startingOnly).AsHttpResponse();
        }

        [Route("pathway_suggest/{name}/{startingOnly}/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetSuggestedPathway(string name, bool startingOnly, string gender, int age)
        {

            var cacheKey = String.Format("PathwayGetAllGrouped-{0}-{1}", gender, age);
            #if !DEBUG
                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return _searchCorrectionService.GetCorrection(JsonConvert.DeserializeObject<List<GroupedPathways>>(cacheValue), name).AsHttpResponse();

                }
            #endif

            var result = await _pathwayService.GetPathways(true, false, gender, age);
            #if !DEBUG
            _cacheManager.Set(cacheKey, result);
            #endif
            return _searchCorrectionService.GetCorrection(JsonConvert.DeserializeObject<List<GroupedPathways>>(result), name).AsHttpResponse();
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
        public async Task<HttpResponseMessage> GetPathwayQuestion(string name, bool startingOnly)
        {
            return await _searchCorrectionService.GetCorrection(name, startingOnly).AsHttpResponse();
        }
    }
}