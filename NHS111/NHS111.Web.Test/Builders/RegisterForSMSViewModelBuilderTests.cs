using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NUnit.Framework;
using RestSharp;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class RegisterForSMSViewModelBuilderTests
    {
        private Mock<IRestClient> _mockRestClient;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp()]
        public void Setup()
        {
            _mockRestClient = new Mock<IRestClient>();
            _mockConfiguration = new Mock<IConfiguration>();
            Mapper.Initialize(cfg => cfg.AddProfile(new DataCaptureApiRequestMappings()));
        }

        [Test]
        public async Task MessageCaseDataCaptureApi_SendsRequestWithCorrectParameters()
        {
            // Arrange
            var builder = new RegisterForSMSViewModelBuilder(_mockConfiguration.Object, _mockRestClient.Object);
            var mobileNumber = "1234567891";
            var endPointUrl = "api/test-endpoint";
            var model = new SendSmsOutcomeViewModel() { MobileNumber = mobileNumber, VerificationCodeInput = new VerificationCodeInputViewModel() {InputValue = "654321"}};
            var expectedRequest = new JsonRestRequest(endPointUrl, Method.POST).AddJsonBody(mobileNumber);
            _mockRestClient.Setup(s => s.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse());

            // Act
            var result = await builder.MessageCaseDataCaptureApi<VerifySMSCodeRequest, SMSEnterVerificationCodeViewDeterminer>(model, endPointUrl);

            // Assert
            _mockRestClient.Verify(m => m.ExecuteTaskAsync(It.Is<RestRequest>(
                r => r.Method == Method.POST
                     && r.Resource == endPointUrl
                     && r.Parameters[0].Value.ToString().Contains(mobileNumber))), Times.Once);
        }
    }
}
