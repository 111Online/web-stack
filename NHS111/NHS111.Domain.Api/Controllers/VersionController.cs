using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;

namespace NHS111.Domain.Api.Controllers
{
    [LogHandleErrorForApi]
    public class VersionController : ApiController
    {
        private readonly IVersionRepository _versionRepository;

        public VersionController(IVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        [HttpGet]
        [Route("version/info")]
        public async Task<JsonResult<VersionInfo>> GetVersionInfo()
        {
            var versionInfo = await _versionRepository.GetInfo();
            return Json(versionInfo);
        }
    }
}