
using System.Collections.Generic;
using System.Linq;

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
    internal class CareAdviceRepositoryTests
    {
        private readonly string[] _keywords = { "Keyword1", "Keyword2" };
        private readonly DispositionCode _dxCode = new DispositionCode("Dx042");
        private readonly AgeCategory _ageCategory = new AgeCategory("child");
        private readonly Gender _gender = new Gender("female");

        private Mock<IGraphRepository> _mockGraph;
        private Mock<IGraphClient> _mockClient;
        private Mock<ICypherFluentQuery> _mockQuery;
        private Mock<ICypherFluentQuery<CareAdviceRepository.CareAdviceFlattened>> _mockTypedQuery;

        private Mock<IRawGraphClient> _mockRawGraphClient;

        [Test]
        public async void GetCareAdvice_WithArgs_BuildsCorrectQuery()
        {

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
            _mockQuery.Setup(q => q.Return(It.IsAny<Expression<Func<ICypherResultItem, ICypherResultItem, CareAdviceRepository.CareAdviceFlattened>>>()))
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

            var expectedMatchClause = "(t:CareAdviceText)-[:hasText*]-(i:InterimCareAdvice)-[:presentsFor]->(o:Outcome)";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.Match(It.Is<string>(s => s == expectedMatchClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_DxCode_Where_Statement()
        {
            SetupMockImplimentations();

            var expectedAndWhereClause = "o.id = \"" + _dxCode.Value + "\"";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);

            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_GenderAndAge_Where_Statement()
        {
            SetupMockImplimentations();
            var expectedAndWhereClause = "i.id =~ \".*-" + _ageCategory.Value + "-" + _gender.Value + "\"";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);
        }

        [Test]
        public async void GetCareAdvice_WithArgs_Builds_ExcludesKeywords_Where_Statement()
        {
            SetupMockImplimentations();
            var expectedAndWhereClause = "(i.excludeKeywords IS null OR NOT (ANY(ex in i.excludeKeywords WHERE ex = \"Keyword1\") OR ANY(ex in i.excludeKeywords WHERE ex = \"Keyword2\")))";
            var sut = new CareAdviceRepository(_mockGraph.Object);
            await sut.GetCareAdvice(_ageCategory, _gender, _keywords, _dxCode);
            _mockQuery.Verify(q => q.AndWhere(It.Is<string>(s => s == expectedAndWhereClause)), Times.Once);

        }

        [Test()]
        public void CareAdviceFlattened_Sort_Child_Items_Sorts_Descendants()
        {
            var flattenedCareAdvice = CreateMockFlattenedCareAdvice();

            var result = flattenedCareAdvice.Sort();

            Assert.AreEqual(4, result.Items.Count);
            Assert.AreEqual(2, result.Items.Where(i => i.Id == "CareAdvice_PARENT_1").Select(i => i.Items.Count).First());
            Assert.AreEqual(2, result.Items.Where(i => i.Id == "CareAdvice_PARENT_1").Select(i => i.Items.Count).First());
        }

        [Test()]
        public void CareAdviceFlattened_Sort_Child_Items_Orders_Correctly()
        {
            var flattenedCareAdvice = CreateMockFlattenedCareAdvice();

            var result = flattenedCareAdvice.Sort();

            Assert.AreEqual("CareAdvice_PARENT_1", result.Items.First().Id);
            Assert.AreEqual("CareAdvice_CHILD_3_2", result.Items[2].Items[1].Id);
        }

        private static CareAdviceRepository.CareAdviceFlattened CreateMockFlattenedCareAdvice()
        {
            return new CareAdviceRepository.CareAdviceFlattened()
            {
                CareAdviceItem = new CareAdvice()
                {
                    Id = "PARENT_CARE-ADVICE-1",
                    Keyword = "TEST_KEYWORD",
                    Title = "TEST_TITLE",
                    Items = new List<CareAdviceText>()
                    {
                    }
                },
                CareAdvcieTextDecendants = new List<CareAdviceTextWithParent>()
                {
                    new CareAdviceTextWithParent() { Id = "CareAdvice_NO_CHILD_2", OrderNo = 3, Text = "CareAdviecNoChild1", ParentId = "PARENT_CARE-ADVICE-1"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_NO_CHILD_1", OrderNo = 1, Text = "CareAdviceParent1", ParentId = "PARENT_CARE-ADVICE-1" },
                    new CareAdviceTextWithParent() { Id = "CareAdvice_CHILD_1", OrderNo = 0, Text = "CareAdviceParent1", ParentId = "CareAdvice_PARENT_1"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_CHILD_2", OrderNo = 1, Text = "CareAdviceParent1", ParentId = "CareAdvice_PARENT_1"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_PARENT_3", OrderNo =2, Text = "CareAdviceParent1", ParentId = "PARENT_CARE-ADVICE-1" },
                    new CareAdviceTextWithParent() { Id = "CareAdvice_CHILD_3_1", OrderNo = 0, Text = "CareAdviceChild1", ParentId = "CareAdvice_PARENT_3"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_CHILD_3_3", OrderNo = 2, Text = "CareAdviceChild3", ParentId = "CareAdvice_PARENT_3"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_CHILD_3_2", OrderNo = 1, Text = "CareAdviceChild2", ParentId = "CareAdvice_PARENT_3"},
                    new CareAdviceTextWithParent() { Id = "CareAdvice_PARENT_1", OrderNo = 0, Text = "CareAdviecParent1", ParentId = "PARENT_CARE-ADVICE-1"},
                }
            };
        }


        [SetUp]
        public void Setup()
        {
            _mockGraph = new Mock<IGraphRepository>();
            _mockClient = new Mock<IGraphClient>();
            _mockQuery = new Mock<ICypherFluentQuery>();
            _mockTypedQuery = new Mock<ICypherFluentQuery<CareAdviceRepository.CareAdviceFlattened>>();
            _mockRawGraphClient = new Mock<IRawGraphClient>();
        }
    }
}