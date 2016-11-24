
namespace NHS111.Business.Services {
    using System.Threading.Tasks;
    using Configuration;
    using Utils.Helpers;

    public class CategoryService
        : ICategoryService {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public CategoryService(IRestfulHelper restfulHelper, IConfiguration configuration) {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<string> GetCategoriesWithPathways() {
            return await _restfulHelper.GetAsync(_configuration.GetCategoriesWithPathwaysUrl());
        }

        public async Task<string> GetCategoriesWithPathways(string gender, int age)
        {
            return await _restfulHelper.GetAsync(_configuration.GetCategoriesWithPathwaysUrl(gender, age));
        }

    }

    public interface ICategoryService {
        Task<string> GetCategoriesWithPathways();
        Task<string> GetCategoriesWithPathways(string gender, int age);
    }
}