using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices.IdealPostcodes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using NHS111.Utils.RestTools;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class LocationResultBuilderTests
    {
        private ILocationResultBuilder _locationResultBuilder;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILoggingRestClient> _mockRestClient;

        [SetUp()]
        public void Setup()
        {
            _mockRestClient = new Mock<ILoggingRestClient>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c.PostcodeSearchByIdUrl).Returns("/location/postcode/api");
            _mockConfiguration.Setup(c => c.PostcodeSubscriptionKey).Returns("xyz");

            _locationResultBuilder = new LocationResultBuilder(_mockRestClient.Object, _mockConfiguration.Object);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Valid_String_Returns_Results()
        {
            var results = new[]
            {
                new AddressLocationResult() {PostCode = "SO30"},
                new AddressLocationResult() {PostCode = "SO31"},
            };

            _mockRestClient.Setup(r => r.ExecuteAsync<List<AddressLocationResult>>(It.IsAny<RestRequest>())).ReturnsAsync(new RestResponse<List<AddressLocationResult>>() { Content = JsonConvert.SerializeObject(results), Data = results.ToList(), ResponseStatus = ResponseStatus.Completed });

            // _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(JsonConvert.SerializeObject(results));

            var locationResults = await _locationResultBuilder.LocationResultByPostCodeBuilder("x");

            Assert.IsInstanceOf(typeof(List<AddressLocationResult>), locationResults);
            Assert.AreEqual(locationResults.Count, 2);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Empty_String_Returns_Empty_List()
        {
            var locationResults = await _locationResultBuilder.LocationResultByPostCodeBuilder(string.Empty);

            Assert.IsInstanceOf(typeof(List<AddressLocationResult>), locationResults);
            Assert.AreEqual(locationResults.Count, 0);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Null_String_Returns_Empty_List()
        {
            var locationResults = await _locationResultBuilder.LocationResultByPostCodeBuilder(null);

            Assert.IsInstanceOf(typeof(List<AddressLocationResult>), locationResults);
            Assert.AreEqual(locationResults.Count, 0);
        }
    }
}
