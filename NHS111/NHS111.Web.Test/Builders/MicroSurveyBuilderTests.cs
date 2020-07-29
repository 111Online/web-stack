using System.Linq;
using System.Threading.Tasks;
using Moq;
using NHS111.Utils.RestTools;
using NUnit.Framework;
using NHS111.Models.Models.Web.MicroSurvey;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture()]
    public class MicroSurveyBuilderTests
    {
        private Mock<ILoggingRestClient> _mockRestClient;
        private Mock<IConfiguration> _mockConfiguration;

        public MicroSurveyBuilderTests()
        {
            _mockRestClient = new Mock<ILoggingRestClient>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(m => m.QualtricsApiToken).Returns("ABC123");
            _mockConfiguration.Setup(m => m.QualtricsRecommendedServiceSurveyId).Returns("testsurveyid");
        }

        [Test]
        public async Task PostMicroSurveyResponse_Calls_Qualtrics_With_Correct_Api_Token()
        {
            var sut = new MicroSurveyBuilder(_mockRestClient.Object, _mockConfiguration.Object);

            await sut.PostMicroSurveyResponse(null);

            _mockRestClient.Verify(m => m.ExecuteAsync(
                It.Is<RestRequest>(c => c.Parameters.Any(s => s.Value == "ABC123"))));
        }

        [Test]
        public async Task PostMicroSurveyResponse_Calls_Qualtrics_With_Method_POST()
        {
            var sut = new MicroSurveyBuilder(_mockRestClient.Object, _mockConfiguration.Object);

            await sut.PostMicroSurveyResponse(null);

            _mockRestClient.Verify(m => m.ExecuteAsync(
                It.Is<RestRequest>(c => c.Method == Method.POST)));
        }

        [Test]
        public async Task PostMicroSurveyResponse_Calls_Qualtrics_With_Uri_That_Is_Correct()
        {
            var sut = new MicroSurveyBuilder(_mockRestClient.Object, _mockConfiguration.Object);

            await sut.PostMicroSurveyResponse(null);

            _mockRestClient.Verify(m => m.ExecuteAsync(
                It.Is<RestRequest>(c => c.Resource == "API/v3/surveys/testsurveyid/responses")));
        }

        [Test]
        public async Task PostMicroSurveyResponse_Calls_Qualtrics_With_Json_Body()
        {
            var sut = new MicroSurveyBuilder(_mockRestClient.Object, _mockConfiguration.Object);

            await sut.PostMicroSurveyResponse(new SurveyResult(){ Values = new SurveyResultValues() { Q1D1 = 1, Q1D2 = new string[] {"1", "4"}} });

            _mockRestClient.Verify(m => m.ExecuteAsync(
                It.Is<RestRequest>(c => c.Parameters.Count == 2)));
        }
    }
}
