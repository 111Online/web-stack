using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace NHS111.Domain.Api.Controllers
{
    [LogHandleErrorForApi]
    public class CategoryController : ApiController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("categories")]
        public async Task<JsonResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            return Json(categories);
        }

        [HttpGet]
        [Route("categories/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<Category>>> GetCategories(string gender, int age)
        {
            var categories = await _categoryRepository.GetCategories(gender, age);
            return Json(categories);
        }

        [HttpGet]
        [Route("categories/pathways")]
        public async Task<JsonResult<IEnumerable<CategoryWithPathways>>> GetCategoriesWithPathways()
        {
            var categoriesWithPathways = await _categoryRepository.GetCategoriesWithPathways();
            return Json(categoriesWithPathways);
        }


        [HttpGet]
        [Route("categories/pathways/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<CategoryWithPathways>>> GetCategoriesWithPathways(string gender, int age)
        {
            var categoriesWithPathways = await _categoryRepository.GetCategoriesWithPathways(gender, age);
            return Json(categoriesWithPathways);
        }

        [HttpGet]
        [Route("category/{category}/pathways")]
        public async Task<JsonResult<CategoryWithPathways>> GetCategoryWithPathways(string category)
        {
            var categoryWithPathways = await _categoryRepository.GetCategoryWithPathways(category);
            return Json(categoryWithPathways);
        }

        [HttpGet]
        [Route("category/{category}/pathways/{gender}/{age}")]
        public async Task<JsonResult<CategoryWithPathways>> GetCategoryWithPathways(string category, string gender, int age)
        {
            var categoryWithPathways = await _categoryRepository.GetCategoryWithPathways(category, gender, age);
            return Json(categoryWithPathways);
        }
    }
}