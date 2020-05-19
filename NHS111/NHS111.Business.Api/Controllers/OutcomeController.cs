
using NHS111.Models.Models.Domain;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace NHS111.Business.Api.Controllers
{
    using Services;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Utils.Attributes;

    [LogHandleErrorForApi]
    public class OutcomeController
        : ApiController
    {

        public OutcomeController(IOutcomeService outcomeService)
        {
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