
namespace NHS111.Domain.Api.Controllers {

    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Repository;
    using Utils.Attributes;
    using Utils.Extensions;

    [LogHandleErrorForApi]
    public class OutcomeController 
        : ApiController {

        public OutcomeController(IOutcomeRepository careAdviceRepository) {
            _outcomeRepository = careAdviceRepository;
        }

        [HttpGet]
        [Route("outcomes/list")]
        public async Task<HttpResponseMessage> ListOutcomes() {
            return await _outcomeRepository.ListOutcomes().AsJson().AsHttpResponse();
        }

        private readonly IOutcomeRepository _outcomeRepository;
    }
}