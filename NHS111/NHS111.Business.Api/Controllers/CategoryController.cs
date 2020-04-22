
using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace NHS111.Business.Api.Controllers
{
    using Services;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryFilter _categoryFilter;

        public CategoryController(ICategoryService categoryService, ICategoryFilter categoryFilter)
        {
            _categoryService = categoryService;
            _categoryFilter = categoryFilter;
        }

        [HttpGet]
        [Route("categories/pathways")]
        public async Task<JsonResult<IEnumerable<CategoryWithPathways>>> GetCategoriesWithPathways()
        {
            var categoriesWithPathways = await _categoryService.GetCategoriesWithPathways();
            return Json(categoriesWithPathways);
        }

        [HttpGet]
        [Route("categories/pathways/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<CategoryWithPathways>>> GetCategoriesWithPathways(string gender, int age)
        {
            var cacheKey = String.Format("GetCategoriesWithPathways-{0}-{1}", gender, age);
#if !DEBUG
            var cacheValue = await _cacheManager.Read(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                return Json(JsonConvert.DeserializeObject<IEnumerable<CategoryWithPathways>>(cacheValue));
            }
#endif

            var categoriesWithPathways = await _categoryService.GetCategoriesWithPathways(gender, age);

#if !DEBUG
            _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(categoriesWithPathways));
#endif
            return Json(categoriesWithPathways);

        }

        [Route("categories/pathways/{gender}/{age}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult<IEnumerable<CategoryWithPathways>>> GetCategoriesWithPathways(string gender, int age, [FromBody] string postcode)
        {

            var cacheKey = String.Format("GetCategoriesWithPathways-{0}-{1}", gender, age);
            IEnumerable<CategoryWithPathways> categoriesWithPathways = new List<CategoryWithPathways>();

#if !DEBUG
            // The cache should ignore any filtering, so that must be done after cache is read or set.
            var cacheValue = await _cacheManager.Read(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                categoriesWithPathways = JsonConvert.DeserializeObject<IEnumerable<CategoryWithPathways>>(cacheValue);
            }
#endif

            if (!categoriesWithPathways.Any())
            {
                categoriesWithPathways = await _categoryService.GetCategoriesWithPathways(gender, age);
            }

#if !DEBUG
            if (string.IsNullOrEmpty(cacheValue) && categoriesWithPathways.Any())
            {
                _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(categoriesWithPathways));
            }
#endif

            if (!string.IsNullOrEmpty(postcode))
            {
                categoriesWithPathways = await _categoryFilter.Filter(categoriesWithPathways, new Dictionary<string, string>() { { "postcode", postcode } });
            }

            return Json(categoriesWithPathways);
        }
    }
}