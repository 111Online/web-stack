using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.Services;
using NHS111.Utils.Attributes;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> Get()
        {
            return await _versionService.GetVersionInfo().AsHttpResponse();
        }
    }
}