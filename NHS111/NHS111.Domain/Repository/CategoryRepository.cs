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

        public async Task<IEnumerable<Category>> GetCategories(string gender, int age)
        {
            var query = await AllCategoriesQuery
                .OptionalMatch("(category)-[:hasPathway]->(categorypathway:Pathway)")
                .Where(string.Join(" and ", new List<string> { GenderIs(gender, "categorypathway"), AgeIsAboveMinimum(age, "categorypathway"), AgeIsBelowMaximum(age, "categorypathway") }))
                .OptionalMatch("(subcategory)-[:hasPathway]->(subcategorypathway:Pathway)")
                .Where(string.Join(" and ", new List<string> { GenderIs(gender, "subcategorypathway"), AgeIsAboveMinimum(age, "subcategorypathway"), AgeIsBelowMaximum(age, "subcategorypathway") }))
                .ReturnDistinct((category, subcategory) => new{ category = category.As<Category>(), subcategory = subcategory.As<Category>() })
                .ResultsAsync;

            var categories = query.Select(c => c.category);
            var subCategories = query.Select(c => c.subcategory);
            return categories.Union(subCategories).Distinct(new CategoryComparer());
        }

        public async Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways()
        {
            var query = await AllCategoriesQuery
                .OptionalMatch("(category)-[:hasPathway]->(categorypathway:Pathway)-[:isDescribedAs]->(categorypathwaymd:PathwayMetaData)") // get the categories pathways and descriptions
                .OptionalMatch("(subcategory)-[:hasPathway]->(subcategorypathway:Pathway)-[:isDescribedAs]->(subcategorypathwaymd:PathwayMetaData)") // get the sub-categories pathways and descriptions
                .Return(
                    (category, categorypathway, categorypathwaymd, subcategory, subcategorypathway, subcategorypathwaymd) =>
                        new CategoryPathwaysFlattened
                        {
                            Category = category.As<Category>(),
                            Pathways = categorypathway.CollectAsDistinct<Pathway>(),
                            PathwaysMetaData = categorypathwaymd.CollectAsDistinct<PathwayMetaData>()
                        })
                .ResultsAsync;

            return query.Select(CategorisePathways);
        }

        public async Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways(string gender, int age)
        {
            var query = AllCategoriesQuery
                .OptionalMatch(
                    "(category)-[:hasPathway]->(categorypathway:Pathway)-[:isDescribedAs]->(categorypathwaymd:PathwayMetaData)") // get the categories pathways and descriptions
                .Where(string.Join(" and ",
                    new List<string>
                    {
                        GenderIs(gender, "categorypathway"),
                        AgeIsAboveMinimum(age, "categorypathway"),
                        AgeIsBelowMaximum(age, "categorypathway")
                    }))
                .OptionalMatch(
                    "(subcategory)-[:hasPathway]->(subcategorypathway:Pathway)-[:isDescribedAs]->(subcategorypathwaymd:PathwayMetaData)") // get the sub-categories pathways and descriptions
                .Where(string.Join(" and ",
                    new List<string>
                    {
                        GenderIs(gender, "subcategorypathway"),
                        AgeIsAboveMinimum(age, "subcategorypathway"),
                        AgeIsBelowMaximum(age, "subcategorypathway")
                    }))
                .Return(
                    (category, categorypathway, categorypathwaymd, subcategory, subcategorypathway, subcategorypathwaymd)
                        =>
                        new CategoryPathwaysFlattened
                        {
                            Category = category.As<Category>(),
                            Pathways = categorypathway.CollectAsDistinct<Pathway>(),
                            PathwaysMetaData = categorypathwaymd.CollectAsDistinct<PathwayMetaData>(),
                            SubCategoriesPathwaysFlattened = subcategory.CollectAsDistinct<Category>()
                                .Select(sc => new CategoryPathwaysFlattened()
                                {
                                    Category = sc,
                                    Pathways = subcategorypathway.CollectAsDistinct<Pathway>(),
                                    PathwaysMetaData = subcategorypathwaymd.CollectAsDistinct<PathwayMetaData>()
                                })
                        });

            var results = await query.ResultsAsync;

            return results.Select(CategorisePathways);
        }

        public async Task<CategoryWithPathways> GetCategoryWithPathways(string id)
        {
            var query = CategoryMatch(id)
                .Return(
                    (c, p, pathwaymd) =>
                        new CategoryPathwaysFlattened
                        {
                            Category = c.As<Category>(),
                            Pathways = p.CollectAsDistinct<Pathway>(),
                            PathwaysMetaData = pathwaymd.CollectAsDistinct<PathwayMetaData>()
                        });

            return CategorisePathways(await query.ResultsAsync.FirstOrDefault());
        }

        public async Task<CategoryWithPathways> GetCategoryWithPathways(string id,string gender, int age)
        {
            var query = CategoryMatch(id)
                .Where(string.Join(" and ", new List<string> { GenderIs(gender, "p"), AgeIsAboveMinimum(age, "p"), AgeIsBelowMaximum(age, "p") }))
                .Return(
                    (c, p, pathwaymd) =>
                        new CategoryPathwaysFlattened
                        {
                            Category = c.As<Category>(),
                            Pathways = p.CollectAsDistinct<Pathway>(),
                            PathwaysMetaData = pathwaymd.CollectAsDistinct<PathwayMetaData>()
                        });

            return CategorisePathways(await query.ResultsAsync.FirstOrDefault());
        }

        private ICypherFluentQuery CategoryMatch(string id)
        {
            return _graphRepository.Client.Cypher
                .Match("(c:Category)")
                .Where(string.Format("c.id = '{0}'", id))
                .With("c")
                .OptionalMatch("(c)-[:hasPathway]->(p:Pathway)-[:isDescribedAs]->(pathwaymd:PathwayMetaData)");
        }

        private ICypherFluentQuery AllCategoriesQuery
        {
            get
            {
                return _graphRepository.Client.Cypher
                    .Match("(c:Category)") // all categories ;
                    .Where("NOT (:Category)<-[:hasParent]-(c)") // filter on parents only
                    .With("c AS parent")
                    .OptionalMatch("(parent)<-[:hasParent]-(c:Category)") // get any sub-categories
                    .With("parent AS category, c AS subcategory");
            }
        }

        private static CategoryWithPathways CategorisePathways(CategoryPathwaysFlattened flattenedData)
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
                    }),
                //SubCategories = flattenedData.SubCategories.Select(flattenedData.)
            };
        }

        private string GenderIs(string gender, string alias)
        {
            return String.Format("({1}.gender is null or {1}.gender = \"\" or {1}.gender = \"{0}\")", gender, alias);
        }


        private string AgeIsAboveMinimum(int age, string alias)
        {
            return
                String.Format(
                    "({1}.minimumAgeInclusive is null or {1}.minimumAgeInclusive = \"\" or {1}.minimumAgeInclusive <= {0})", age, alias);
        }

        private string AgeIsBelowMaximum(int age, string alias)
        {
            return
                String.Format(
                    "({1}.maximumAgeExclusive is null or {1}.maximumAgeExclusive = \"\" or {0} <{1}.maximumAgeExclusive)", age, alias);
        }
    }

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetCategories(string gender, int age);

        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways(string gender, int age);
        Task<IEnumerable<CategoryWithPathways>> GetCategoriesWithPathways();

        Task<CategoryWithPathways> GetCategoryWithPathways(string category, string gender, int age);
        Task<CategoryWithPathways> GetCategoryWithPathways(string category);
    }

    public class CategoryComparer : IEqualityComparer<Category>
    {

        public bool Equals(Category x, Category y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Category obj)
        {
            return obj.Id.GetHashCode();
        }
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
            if (obj.DigitalDescription == null)
                return obj.PathwayNo.GetHashCode();
            return obj.DigitalDescription.GetHashCode();
        }
    }
}
