﻿using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Domain.Repository;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> GetCategories()
        {
            return await _categoryRepository.GetCategories().AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("categories/pathways")]
        public async Task<HttpResponseMessage> GetCategoriesWithPathways()
        {
            return await _categoryRepository.GetCategoriesWithPathways().AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("category/{category}/pathways")]
        public async Task<HttpResponseMessage> GetCategoryWithPathways(string category)
        {
            return await _categoryRepository.GetCategoryWithPathways(category).AsJson().AsHttpResponse();
        }
    }
}