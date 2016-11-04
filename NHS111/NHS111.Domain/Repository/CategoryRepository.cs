using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IGraphRepository _graphRepository;

        public CategoryRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _graphRepository.Client.Cypher
                .Match("(c:Category)")
                .Return(c => c.As<Category>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways()
        {
            return await _graphRepository.Client.Cypher
                .Match("(c:Category)-[:hasPathway]->(p:Pathway)")
                .Return(q => new CategoryWithPathways { Category = Return.As<Category>("c"), Pathways = Return.As<List<Pathway>>("collect(p)") })
                .ResultsAsync;
        }

        public async Task<CategoryWithPathways> GetCategoryWithPathways(string category)
        {
            return await _graphRepository.Client.Cypher
                .Match("(c:Category)-[:hasPathway]->(p:Pathway)")
                .Where(string.Format("c.title = '{0}'", category))
                .Return(q => new CategoryWithPathways { Category = Return.As<Category>("c"), Pathways = Return.As<List<Pathway>>("collect(p)") })
                .ResultsAsync
                .FirstOrDefault();
        }
    }

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways();

        Task<CategoryWithPathways> GetCategoryWithPathways(string category);
    }
}
