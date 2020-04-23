using Moq;
using NHS111.Domain.Repository;
using NHS111.Features;
using NHS111.Models.Models.Domain;

namespace NHS111.Domain.Integration.Test.Repository
{
    public class RepositoryTestBase
    {
        protected readonly IGraphRepository GraphRepository;

        protected readonly Mock<IPathwaysWhiteListFeature> MockPathwaysWhiteListFeature = new Mock<IPathwaysWhiteListFeature>();

        protected readonly Pathway[] Pathways =
        {
            new Pathway { Id = "P1", Title = "Live Pathway One", PathwayNo = "LPW101", Module = null},
            new Pathway { Id = "P2", Title = "Pathway One", PathwayNo = "PW101", Module = "1" },
            new Pathway { Id = "P3", Title = "Live Pathway Two", PathwayNo = "LPW102", Module = "1" },
            new Pathway { Id = "P4", Title = "Pathway Two", PathwayNo = "PW102", Module = "1", Gender = "Male", MinimumAgeInclusive = 16, MaximumAgeExclusive = 200 },
            new Pathway { Id = "P5", Title = "Pathway Three", PathwayNo = "PW103", Module = "1", Gender = "Female", MinimumAgeInclusive = 16, MaximumAgeExclusive = 200 },
            new Pathway { Id = "P6", Title = "Pathway Three", PathwayNo = "PW103", Module = "1", Gender = "Female", MinimumAgeInclusive = 16, MaximumAgeExclusive = 200 },
            new Pathway { Id = "P7", Title = "Live Pathway Three", PathwayNo = "LPW103", Module = "1", Gender = "Male", MinimumAgeInclusive = 16, MaximumAgeExclusive = 200 },
            new Pathway { Id = "P8", Title = "Live Pathway Three", PathwayNo = "LPW103", Module = "1", Gender = "Female", MinimumAgeInclusive = 16, MaximumAgeExclusive = 200 }
        };

        public RepositoryTestBase()
        {
            GraphRepository = new GraphRepository(new Configuration.Configuration());
        }

        public void DeleteAllNodesFromNeo4jDatabase()
        {

            GraphRepository.Client.Cypher
                .Match("(n)")
                .OptionalMatch("(n)-[r]-()")
                .Delete("n,r")
                .ExecuteWithoutResults();
        }

        public void CreateTestNeo4jData()
        {
            foreach (var pathway in Pathways)
                CreatePathwayNode(pathway);
        }

        private void CreatePathwayNode(Pathway newPathway)
        {
            GraphRepository.Client.Cypher
                .Create("(p:Pathway {newPathway})")
                .WithParam("newPathway", newPathway)
                .ExecuteWithoutResults();
        }
    }
}
