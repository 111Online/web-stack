
namespace NHS111.Domain.Test.Repository {
    using System;
    using System.Linq.Expressions;
    using Domain.Repository;
    using Models.Models.Domain;
    using Moq;
    using Neo4jClient;
    using Neo4jClient.Cypher;
    using NUnit.Framework;

    [TestFixture]
    internal class CareAdviceRepositoryTests {
        private readonly string[] _keywords = { "Keyword1", "Keyword2" };
        private readonly string _dxCode = "Dx042";
        private readonly string _ageCategory = "child";
        private readonly string _gender = "female";

        private Mock<IGraphRepository> _mockGraph;
        private Mock<IGraphClient> _mockClient;
        private Mock<ICypherFluentQuery> _mockQuery;
        private Mock<ICypherFluentQuery<CareAdvice>> _mockTypedQuery;

        [Test]
        public async void GetCareAdvice_WithArgs_BuildsCorrectQuery() {

            Setup();

            _mockGraph.Setup(g => g.Client).Returns(_mockClient.Object);
            _mockClient.Setup(c => c.Cypher).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Match(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Where(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.AndWhere(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Return(It.IsAny<Expression<Func<ICypherResultItem, CareAdvice>>>())).Returns(_mockTypedQuery.Object);

            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.Where(It.Is<string>(s => s.Contains(_keywords[0]) && s.Contains(_keywords[1]))), Times.Once);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s.Contains(_dxCode))), Times.Once);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s.Contains(_ageCategory) && s.Contains(_gender))), Times.Once);

        }

        [SetUp]
        public void Setup() {
            _mockGraph = new Mock<IGraphRepository>();
            _mockClient = new Mock<IGraphClient>();
            _mockQuery = new Mock<ICypherFluentQuery>();
            _mockTypedQuery = new Mock<ICypherFluentQuery<CareAdvice>>();
        }
    }
}