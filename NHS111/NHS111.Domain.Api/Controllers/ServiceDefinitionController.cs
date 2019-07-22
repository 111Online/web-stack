using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;

namespace NHS111.Domain.Api.Controllers
{
    public class ServiceDefinitionController : ApiController
    {
        private readonly IServiceDefinitionRepository _serviceDefinitionRepository;

        public ServiceDefinitionController(IServiceDefinitionRepository serviceDefinitionRepository)
        {
            _serviceDefinitionRepository = serviceDefinitionRepository;
        }

        [HttpGet]
        [Route("servicedefinition/{pathwayNo}")]
        public async Task<JsonResult<ServiceDefinition>> GetServiceDefinition(string pathwayNo)
        {
            var pathway = await _serviceDefinitionRepository.GetServiceDefinition(pathwayNo);
            return Json(pathway);
        }
    }
}
