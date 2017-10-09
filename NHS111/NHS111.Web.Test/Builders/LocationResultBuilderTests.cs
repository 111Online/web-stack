using System.Collections.Generic;
using System.Net;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Location;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class LocationResultBuilderTests
    {
        private ILocationResultBuilder _locationResultBuilder;
        private Mock<IRestClient> _mockRestclient;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp()]
        public void Setup()
        {
            _mockRestclient = new Mock<IRestClient>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c.PostcodeSearchByIdApiUrl).Returns("/location/postcode/api");
            _mockConfiguration.Setup(c => c.PostcodeSubscriptionKey).Returns("xyz");

            _locationResultBuilder = new LocationResultBuilder(_mockConfiguration.Object, _mockRestclient.Object);
        }

        [Test()]
        public async void AddressByPostCodeBuilder_With_Valid_String_Returns_Results()
        {
            var results = new List<AddressLocationResult>()
            {
                new AddressLocationResult() {PostCode = "SO30"},
                new AddressLocationResult() {PostCode = "SO31"},
            };

            _mockRestclient.Setup(r => r.ExecuteTaskAsync<List<AddressLocationResult>>(It.IsAny<RestRequest>())).ReturnsAsync(new RestResponse<List<AddressLocationResult>>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = results });

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
