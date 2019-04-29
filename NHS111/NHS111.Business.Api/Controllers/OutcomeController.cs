
using System.Collections.Generic;
using System.Web.Http.Results;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Api.Controllers {
    using System.Threading.Tasks;
    using System.Web.Http;
    using Services;
    using Utils.Attributes;
    using Utils.Extensions;

    [LogHandleErrorForApi]
    public class OutcomeController
        : ApiController {

        public OutcomeController(IOutcomeService outcomeService) {
            _outcomeService = outcomeService;
        }

        [HttpGet]
        [Route("outcome/list")]
        public async Task<JsonResult<IEnumerable<Outcome>>> List()
        {
            var outcomes = await _outcomeService.List();
            return Json(outcomes);
        }

        private readonly IOutcomeService _outcomeService;
    }
}