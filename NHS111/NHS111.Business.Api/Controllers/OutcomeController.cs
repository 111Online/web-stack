
namespace NHS111.Business.Api.Controllers {

    using System.Net.Http;
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
        public async Task<HttpResponseMessage> List() {
            return await _outcomeService.List().AsHttpResponse();
        }

        private readonly IOutcomeService _outcomeService;
    }
}