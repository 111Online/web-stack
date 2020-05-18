using Moq;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.WhiteListPopulator;
using NHS111.Models.Models.Web.CCG;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using NHS111.Utils.RestTools;

namespace NHS111.Business.DOS.Test.WhiteListPopulator
{
    [TestFixture]
    public class ReferralServicesWhiteListPopulatorTests
    {
        private Mock<ILoggingRestClient> _mockCCGRestAPI = new Mock<ILoggingRestClient>();
        private Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
        private readonly string _localServiceIdWhiteListUrl = "http://localhost/api/ccg/details/{0}";

        [Test]
        public void ReferralServicesWhiteListPopulator_Success()
        {
            var postcode = "so302uh";
            var whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);

            _mockConfiguration.Setup(c => c.CCGApiGetCCGDetailsByPostcode).Returns(whitelistUrl);
            _mockCCGRestAPI.Setup(r => r.ExecuteAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            ReferralServicesWhiteListPopulator sut = new ReferralServicesWhiteListPopulator(_mockCCGRestAPI.Object, _mockConfiguration.Object);
            var result = sut.PopulateCCGWhitelist(postcode);

            Assert.AreEqual(4, result.Result.Count);
            Assert.AreEqual("123", result.Result[0]);
            Assert.AreEqual("456", result.Result[1]);
            Assert.AreEqual("789", result.Result[2]);
            Assert.AreEqual("1419419102", result.Result[3]);
        }

        [Test]
        public void ReferralServicesWhiteListPopulator_NullData()
        {
            var postcode = "so302uh";
            var whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);

            _mockConfiguration.Setup(c => c.CCGApiGetCCGDetailsByPostcode).Returns(whitelistUrl);
            _mockCCGRestAPI.Setup(r => r.ExecuteAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = null }));

            ReferralServicesWhiteListPopulator sut = new ReferralServicesWhiteListPopulator(_mockCCGRestAPI.Object, _mockConfiguration.Object);
            var result = sut.PopulateCCGWhitelist(postcode);

            Assert.AreEqual(0, result.Result.Count);
        }

        [Test]
        public void ReferralServicesWhiteListPopulator_EmptyData()
        {
            var postcode = "so302uh";
            var whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);

            _mockConfiguration.Setup(c => c.CCGApiGetCCGDetailsByPostcode).Returns(whitelistUrl);
            _mockCCGRestAPI.Setup(r => r.ExecuteAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel() }));

            ReferralServicesWhiteListPopulator sut = new ReferralServicesWhiteListPopulator(_mockCCGRestAPI.Object, _mockConfiguration.Object);
            var result = sut.PopulateCCGWhitelist(postcode);

            Assert.AreEqual(0, result.Result.Count);
        }

        [Test]
        public async void ReferralServicesWhiteListPopulator_HTTPServerError()
        {
            var postcode = "so302uh";
            var whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);

            _mockConfiguration.Setup(c => c.CCGApiGetCCGDetailsByPostcode).Returns(whitelistUrl);
            _mockCCGRestAPI.Setup(r => r.ExecuteAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.InternalServerError }));

            ReferralServicesWhiteListPopulator sut = new ReferralServicesWhiteListPopulator(_mockCCGRestAPI.Object, _mockConfiguration.Object);
            try
            {
                var result = await sut.PopulateCCGWhitelist(postcode);

                Assert.Fail();
            }
            catch (HttpException e)
            {
                Assert.IsInstanceOf<HttpException>(e);
            }

        }

        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }
    }
}
