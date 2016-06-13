
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
    [Ignore]
    internal class CareAdviceRepositoryTests {
        private readonly string[] _keywords = { "Keyword1", "Keyword2" };
        private readonly DispositionCode _dxCode = new DispositionCode("Dx042");
        private readonly AgeCategory _ageCategory = new AgeCategory("child");
        private readonly Gender _gender = new Gender("female");

        private Mock<IGraphRepository> _mockGraph;
        private Mock<IGraphClient> _mockClient;
        private Mock<ICypherFluentQuery> _mockQuery;
        private Mock<ICypherFluentQuery<object>> _mockTypedQuery;

        private Mock<IRawGraphClient> _mockRawGraphClient;

        [Test]
        public async void GetCareAdvice_WithArgs_BuildsCorrectQuery() {

            SetupMockImplimentations();

            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.Where(It.Is<string>(s => s.Contains(_keywords[0]) && s.Contains(_keywords[1]))), Times.Once);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s.Contains(_dxCode.Value))), Times.Once);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s.Contains(_ageCategory.Value) && s.Contains(_gender.Value))), Times.Once);

        }

        private void SetupMockImplimentations()
        {
            _mockGraph.Setup(g => g.Client).Returns(_mockClient.Object);
            _mockClient.Setup(c => c.Cypher).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Match(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Where(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.AndWhere(It.IsAny<string>())).Returns(_mockQuery.Object);
            _mockQuery.Setup(q => q.Return(It.IsAny<Expression<Func<ICypherResultItem, ICypherResultItem, object>>>()))
                .Returns(_mockTypedQuery.Object);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_Keywords_Where_Statement()
        {
            SetupMockImplimentations();

            var expectedKeywordsWhereClause = "i.keyword in [\"Keyword1\",\"Keyword2\"]";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.Where(It.Is<string>(s => s == expectedKeywordsWhereClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_Match_Statement()
        {
            SetupMockImplimentations();

            var expectedMatchClause = "(i:InterimCareAdvice)-[:presentsFor]->(o:Outcome)";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.Match(It.Is<string>(s => s == expectedMatchClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_DxCode_Where_Statement()
        {
            SetupMockImplimentations();

            var expectedAndWhereClause = "o.id = \""+_dxCode.Value +"\"";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_GenderAndAge_Where_Statement()
        {
            SetupMockImplimentations();
            var expectedAndWhereClause = "i.id =~ \".*-" + _ageCategory.Value + "-" +_gender.Value+ "\"";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_ExcludesKeywords_Where_Statement()
        {
            SetupMockImplimentations();
            var expectedAndWhereClause = "NOT (ANY(ex in i.excludeKeywords WHERE ex = \"Keyword1\") OR ANY(ex in i.excludeKeywords WHERE ex = \"Keyword2\"))";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);

        }

        

        [SetUp]
        public void Setup() {
            _mockGraph = new Mock<IGraphRepository>();
            _mockClient = new Mock<IGraphClient>();
            _mockQuery = new Mock<ICypherFluentQuery>();
            _mockTypedQuery = new Mock<ICypherFluentQuery<object>>();
            _mockRawGraphClient = new Mock<IRawGraphClient>();
        }
    }
}