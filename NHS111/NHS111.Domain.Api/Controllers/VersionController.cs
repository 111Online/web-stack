using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NHS111.Domain.Repository;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> GetVersionInfo()
        {
            return await _versionRepository.GetInfo().AsJson().AsHttpResponse();
        }
    }
}