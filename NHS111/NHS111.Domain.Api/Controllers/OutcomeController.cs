
using NHS111.Models.Models.Domain;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace NHS111.Domain.Api.Controllers
{
    using Repository;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Utils.Attributes;

    [LogHandleErrorForApi]
    public class OutcomeController
        : ApiController
    {

        public OutcomeController(IOutcomeRepository careAdviceRepository)
        {
            _outcomeRepository = careAdviceRepository;
        }

        [HttpGet]
        [Route("outcomes/list")]
        public async Task<JsonResult<IEnumerable<Outcome>>> ListOutcomes()
        {
            var outcomes = await _outcomeRepository.ListOutcomes();
            return Json(outcomes);
        }

        private readonly IOutcomeRepository _outcomeRepository;
    }
}