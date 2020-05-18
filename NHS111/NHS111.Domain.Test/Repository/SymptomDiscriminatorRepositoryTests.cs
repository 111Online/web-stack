
namespace NHS111.Domain.Test.Repository
{
    using Domain.Repository;
    using Models.Models.Domain;
    using Moq;
    using Neo4jClient;
    using Neo4jClient.Cypher;
    using NUnit.Framework;
    using System;
    using System.Linq.Expressions;

    [TestFixture]
    public class SymptomDiscriminatorRepositoryTests
    {

        private Mock<IGraphRepository> _mockGraph;
        private Mock<IGraphClient> _mockClient;
        private Mock<ICypherFluentQuery> _mockQuery;
        private Mock<ICypherFluentQuery<SymptomDiscriminator>> _mockTypedQuery;

        [SetUp]
        public void SetUp()
        {
            _mockGraph = new Mock<IGraphRepository>();
            _mockClient = new Mock<IGraphClient>();
            _mockQuery = new Mock<ICypherFluentQuery>();
            _mockTypedQuery = new Mock<ICypherFluentQuery<SymptomDiscriminator>>();
        }

        [Test]
        public void Get_Always_QueriesSDNode()
        {

            _mockGraph.Setup(g => g.Client).Returns(_mockClient.Object);
            _mockClient.Setup(c => c.Cypher).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Match(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Return(It.IsAny<Expression<Func<ICypherResultItem, SymptomDiscriminator>>>()))
                .Returns(_mockTypedQuery.Object);

            var sut = new SymptomDiscriminatorRepository(_mockGraph.Object);
            var sd = sut.Get(123);

            _mockQuery.Verify(q => q.Match(It.Is<string>(s => s.Contains(SymptomDiscriminatorRepository.SymptomDiscriminatorNodeName))));
        }
    }
}
