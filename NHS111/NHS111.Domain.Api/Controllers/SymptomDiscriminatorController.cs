using System.Web.Http.Results;

namespace NHS111.Domain.Api.Controllers {
    using System.Threading.Tasks;
    using System.Web.Http;
    using Models.Models.Domain;
    using Repository;
    using Utils.Attributes;

    [LogHandleErrorForApi]
    public class SymptomDiscriminatorController: ApiController {

        public SymptomDiscriminatorController(ISymptomDiscriminatorRepository repository) {
            _repository = repository;
        }

        [Route("symptomdiscriminator/{id}")]
        public async Task<JsonResult<SymptomDiscriminator>> Get(int id)
        {
            var sd = await _repository.Get(id);
            return Json(sd);
        }

        private readonly ISymptomDiscriminatorRepository _repository;
    }
}