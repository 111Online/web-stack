using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class AddressBuilderTests
    {
        private IAddressBuilder _addressBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp()]
        public void Setup()
        {
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c.PostcodeSearchByIdApiUrl).Returns("/location/postcode/api");
            _mockConfiguration.Setup(c => c.PostcodeSubscriptionKey).Returns("xyz");

            _addressBuilder = new AddressBuilder(_mockRestfulHelper.Object, _mockConfiguration.Object);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Valid_String_Returns_Results()
        {
            var addresses = new[]
            {
                new LocationResult() {Postcode = "SO30"},
                new LocationResult() {Postcode = "SO31"},
            };

            _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(JsonConvert.SerializeObject(addresses));

            var addressResults = await _addressBuilder.AddressByPostCodeBuilder("x");

            Assert.IsInstanceOf(typeof(List<LocationResult>), addressResults);
            Assert.AreEqual(addressResults.Count, 2);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Empty_String_Returns_Empty_List()
        {
            var addressResults = await _addressBuilder.AddressByPostCodeBuilder(string.Empty);

            Assert.IsInstanceOf(typeof(List<LocationResult>), addressResults);
            Assert.AreEqual(addressResults.Count, 0);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Null_String_Returns_Empty_List()
        {
            var addressResults = await _addressBuilder.AddressByPostCodeBuilder(null);

            Assert.IsInstanceOf(typeof(List<LocationResult>), addressResults);
            Assert.AreEqual(addressResults.Count, 0);
        }
    }
}
