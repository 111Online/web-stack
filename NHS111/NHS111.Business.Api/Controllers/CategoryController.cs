
using System;
using NHS111.Utils.Cache;

namespace NHS111.Business.Api.Controllers {
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Services;
    using Utils.Extensions;

    public class CategoryController : ApiController {
        private readonly ICategoryService _categoryService;
        private readonly ICacheManager<string, string> _cacheManager;

        public CategoryController(ICategoryService categoryService, ICacheManager<string, string> cacheManager)
        {
            _categoryService = categoryService;
            _cacheManager = cacheManager;
        }

        [HttpGet]
        [Route("categories/pathways")]
        public async Task<HttpResponseMessage> GetCategoriesWithPathways() {
            return await _categoryService.GetCategoriesWithPathways().AsHttpResponse();
        }

        [HttpGet]
        [Route("categories/pathways/{gender}/{age}")]
        public async Task<HttpResponseMessage> GetCategoriesWithPathways(string gender, int age)
        {
            var cacheKey = String.Format("GetCategoriesWithPathways-{0}-{1}", gender, age);
            #if !DEBUG
                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
            #endif

            var result = await _categoryService.GetCategoriesWithPathways(gender, age);
            #if !DEBUG
              _cacheManager.Set(cacheKey, result);
            #endif
            return result.AsHttpResponse();
        }
    }
}