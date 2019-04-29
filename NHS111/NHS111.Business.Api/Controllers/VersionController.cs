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
    public class VersionController : ApiController
    {
        private readonly IVersionService _versionService;
        private readonly ICacheManager<string, string> _cacheManager;

        public VersionController(IVersionService versionService, ICacheManager<string, string> cacheManager)
        {
            _versionService = versionService;
            _cacheManager = cacheManager;
        }

        [Route("version")]
        public async Task<JsonResult<VersionInfo>> Get(string cacheKey = null)
        {
#if !DEBUG
                cacheKey = cacheKey ?? "version-info";

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<VersionInfo>(cacheValue));
                }
#endif

            var version = await _versionService.GetVersionInfo();
#if !DEBUG
                _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(version));
#endif

            var versionInfo = await _versionService.GetVersionInfo();
            return Json(versionInfo);
        }
    }
}