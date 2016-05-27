using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class SurgeryBuilderTests
    {
        private ISurgeryBuilder _surgeryBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp()]
        public void Setup()
        {
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c.GPSearchApiUrl).Returns("search/api");
            _mockConfiguration.Setup(c => c.GPSearchByIdUrl).Returns("searchById/api");

            _surgeryBuilder = new SurgeryBuilder(_mockRestfulHelper.Object, _mockConfiguration.Object);
        }

        [Test()]
        public async void SearchSurgeryBuilder_With_Valid_String_Returns_Results()
        {
            var surgeries = new[]
            {
                new Surgery() {SurgeryId = "1"},
                new Surgery() {SurgeryId = "2"},
            };

            _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(JsonConvert.SerializeObject(surgeries));

            var surgeryResults = await _surgeryBuilder.SearchSurgeryBuilder("x");

            Assert.IsInstanceOf(typeof(List<Surgery>), surgeryResults);
            Assert.AreEqual(surgeryResults.Count, 2);
        }

        [Test()]
        public async void SearchSurgeryBuilder_With_Empty_String_Returns_Empty_List()
        {
            var surgeryResults = await _surgeryBuilder.SearchSurgeryBuilder(string.Empty);

            Assert.IsInstanceOf(typeof(List<Surgery>), surgeryResults);
            Assert.AreEqual(surgeryResults.Count, 0);
        }

        [Test()]
        public async void SearchSurgeryBuilder_With_Null_String_Returns_Empty_List()
        {
            var surgeryResults = await _surgeryBuilder.SearchSurgeryBuilder(null);

            Assert.IsInstanceOf(typeof(List<Surgery>), surgeryResults);
            Assert.AreEqual(surgeryResults.Count, 0);
        }

        [Test()]
        public async void SearchSurgeryById_With_Valid_String_Returns_Surgery()
        {
            var surgery = new Surgery() {SurgeryId = "1"};

            _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(JsonConvert.SerializeObject(surgery));

            var surgeryResult = await _surgeryBuilder.SurgeryByIdBuilder("1");

            Assert.IsInstanceOf(typeof(Surgery), surgery);
            Assert.AreEqual(surgery.SurgeryId, "1");
        }

        [Test()]
        public async void SearchSurgeryById_With_Empty_String_Returns_UKN()
        {
            var surgery = await _surgeryBuilder.SurgeryByIdBuilder(string.Empty);
            Assert.AreEqual(surgery.SurgeryId, "UKN");
        }

        [Test()]
        public async void SearchSurgeryById_With_Null_String_Returns_UKN()
        {
            var surgery = await _surgeryBuilder.SurgeryByIdBuilder(null);
            Assert.AreEqual(surgery.SurgeryId, "UKN");
        }
    }
}
