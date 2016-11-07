using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
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
            var query = await _graphRepository.Client.Cypher
                .Match(CategoryMatch)
                .Return(
                    (c, p, pmd) =>
                        new CategoryPathwaysFlattened
                        {
                            Category = c.As<Category>(),
                            Pathways = p.CollectAs<Pathway>(),
                            PathwaysMetaData = pmd.CollectAs<PathwayMetaData>()
                        })
                .ResultsAsync;

            return query.Select(CategroisePathways);

        }

        public async Task<CategoryWithPathways> GetCategoryWithPathways(string category)
        {
            var query = _graphRepository.Client.Cypher
                .Match(CategoryMatch)
                .Where(string.Format("c.title = '{0}'", category))
                .Return(
                    (c, p, pmd) =>
                        new CategoryPathwaysFlattened
                        {
                            Category = c.As<Category>(),
                            Pathways = p.CollectAs<Pathway>(),
                            PathwaysMetaData = pmd.CollectAs<PathwayMetaData>()
                        });

            return CategroisePathways(await query.ResultsAsync.FirstOrDefault());
        }

        private static string CategoryMatch
        {
            get { return "(c:Category)-[:hasPathway]->(p:Pathway)-[:isDescribedAs]->(pmd:PathwayMetaData)"; }
        }

        private static CategoryWithPathways CategroisePathways(CategoryPathwaysFlattened flattenedData)
        {
            return new CategoryWithPathways
            {
                Category = flattenedData.Category,
                Pathways = flattenedData.Pathways
                    .Distinct(new PathwayComparer())
                    .Select(p => new PathwayWithDescriptions()
                    {
                        Pathway = p,
                        PathwayDescriptions = flattenedData.PathwaysMetaData
                            .Distinct(new PathwayMetaDataComparer())
                            .Where(pd => pd.PathwayNo == p.PathwayNo)
                    })
            };
        }
    }

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways();

        Task<CategoryWithPathways> GetCategoryWithPathways(string category);
    }

    public class PathwayComparer : IEqualityComparer<Pathway>
    {

        public bool Equals(Pathway x, Pathway y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Pathway obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class PathwayMetaDataComparer : IEqualityComparer<PathwayMetaData>
    {

        public bool Equals(PathwayMetaData x, PathwayMetaData y)
        {
            return x.PathwayNo == y.PathwayNo && x.DigitalDescription == y.DigitalDescription;
        }

        public int GetHashCode(PathwayMetaData obj)
        {
            return obj.DigitalDescription.GetHashCode();
        }
    }
}
