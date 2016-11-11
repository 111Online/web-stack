
namespace NHS111.Business.Api.Controllers {
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Services;
    using Utils.Extensions;

    public class CategoryController : ApiController {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("categories/pathways")]
        public async Task<HttpResponseMessage> GetCategoriesWithPathways() {
            return await _categoryService.GetCategoriesWithPathways().AsHttpResponse();
        }
    }
}