
using NHS111.Models.Models.Domain;
using NHS111.Utils.RestTools;
using RestSharp;
using System.Collections.Generic;

namespace NHS111.Business.Services
{
    using Configuration;
    using System.Threading.Tasks;

    public class CategoryService
        : ICategoryService
    {
        private readonly ILoggingRestClient _restClient;
        private readonly IConfiguration _configuration;

        public CategoryService(ILoggingRestClient restClientDomainApi, IConfiguration configuration)
        {
            _restClient = restClientDomainApi;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways()
        {
            var categories = await _restClient.ExecuteAsync<IEnumerable<CategoryWithPathways>>(new JsonRestRequest(_configuration.GetCategoriesWithPathwaysUrl(), Method.GET));
            return categories.Data;
        }

        public async Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways(string gender, int age)
        {
            var categories = await _restClient.ExecuteAsync<IEnumerable<CategoryWithPathways>>(new JsonRestRequest(_configuration.GetCategoriesWithPathwaysUrl(gender, age), Method.GET));
            return categories.Data;
        }

    }

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways();
        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways(string gender, int age);
    }
}