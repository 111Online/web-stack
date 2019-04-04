using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Web.Presentation.Builders.Tests
{
    [TestFixture()]
    public class CareAdviceBuilderTests
    {
        Mock<IRestClient> _restClient;
        Mock<IConfiguration> _configuration;

        private string MOCK_GetBusinessApiCareAdviceUrl = "http://GetBusinessApiCareAdviceUrl.com";
        private string MOCK_GetBusinessApiInterimCareAdviceUrl = "http://GetBusinessApiInterimCareAdviceUrl.com";
        private const string TEST_CAREADVICE_ID = "Cx220985-Adult-Male";
        private const string TEST_CAREADVICE_ITEM_FIRST = "Care advice test first";
        private const string TEST_CAREADVICE_ITEM_SECOND = "Care advice test second";

        private const string TEST_CONTENT =
            "[{'id':'" + TEST_CAREADVICE_ID + "','title':null,'excludeTitle':null,'items':" +
            "[{'text':'" + TEST_CAREADVICE_ITEM_FIRST + "'},{'text':'" + TEST_CAREADVICE_ITEM_SECOND + "'}]}," +
            "{'id':'Cx220986-Adult-Male','title':null,'excludeTitle':null,'items':[{'text':'Single care advice item'}]}]";

        [SetUp()]
        public void SetUp()
        {
            _configuration = new Mock<IConfiguration>();
            _restClient = new Mock<IRestClient>();

            _configuration.Setup(c => c.GetBusinessApiCareAdviceUrl(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(MOCK_GetBusinessApiCareAdviceUrl);

            _configuration.Setup(c => c.GetBusinessApiInterimCareAdviceUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(MOCK_GetBusinessApiInterimCareAdviceUrl);

            _restClient.Setup(r => r.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<RestRequest>())).ReturnsAsync(new RestResponse<IEnumerable<CareAdvice>>() { Content = TEST_CONTENT, Data = JsonConvert.DeserializeObject<IEnumerable<CareAdvice>>(TEST_CONTENT), ResponseStatus = ResponseStatus.Completed });

            _restClient.Setup(r => r.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<RestRequest>())).ReturnsAsync(new RestResponse<IEnumerable<CareAdvice>>() { Content = TEST_CONTENT, Data = JsonConvert.DeserializeObject<IEnumerable<CareAdvice>>(TEST_CONTENT), ResponseStatus = ResponseStatus.Completed });
        }

        [Test()]
        public async void FillCareAdviceBuilderTest_Passes_Correct_Keywords()
        {
            var careAdviceBuilerToTest = new CareAdviceBuilder(_restClient.Object, _configuration.Object);

            await careAdviceBuilerToTest.FillCareAdviceBuilder("Dx11", "Adult", "Male",
                new List<string>() {TEST_CAREADVICE_ITEM_FIRST, TEST_CAREADVICE_ITEM_SECOND});

            var expectedKeywordsString = TEST_CAREADVICE_ITEM_FIRST + "|" + TEST_CAREADVICE_ITEM_SECOND;

            _configuration.Verify(c => c.GetBusinessApiInterimCareAdviceUrl(
                It.Is<string>(s => s == "Dx11"),
                It.Is<string>(s => s == "Adult"),
                It.Is<string>(s => s == "Male")));
        }

        [Test()]
        public async void FillCareAdviceBuilderTest_Builds_expected_CareAdvice()
        {
            var careAdviceBuilerToTest = new CareAdviceBuilder(_restClient.Object, _configuration.Object);

            var result = await careAdviceBuilerToTest.FillCareAdviceBuilder("Dx11", "Adult", "Male",
                new List<string>() { TEST_CAREADVICE_ITEM_FIRST, TEST_CAREADVICE_ITEM_SECOND });

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(TEST_CAREADVICE_ID, result.First().Id);

            Assert.AreEqual(2, result.First().Items.Count());
            Assert.AreEqual(TEST_CAREADVICE_ITEM_FIRST, result.First().Items.First().Text);
            Assert.AreEqual(TEST_CAREADVICE_ITEM_SECOND, result.First().Items.Last().Text);


        }
    }
}
