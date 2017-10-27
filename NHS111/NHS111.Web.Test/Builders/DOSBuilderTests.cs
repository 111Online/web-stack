using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Notifier;
using NUnit.Framework;
namespace NHS111.Web.Presentation.Builders.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;

    [TestFixture()]
    public class DOSBuilderTests
    {

        private Mock<IMappingEngine> _mappingEngine;
        private Mock<ICareAdviceBuilder> _mockCareAdviceBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<ICacheManager<string, string>> _mockCacheManager;
        private Mock<INotifier<string>> _mockNotifier;
        private DOSBuilder _dosBuilder;
        private Mock<ISurgeryBuilder> _mockSurgeryBuilder;
        private Mock<IITKMessagingFeature> _mockItkMessagingFeature;

        private string _mockPathwayURL = "PW755";

        private string _expectedBusinessApiPathwaySymptomGroupUrl;
    
        [SetUp()]
        public void Setup()
        {
            _mappingEngine = new Mock<IMappingEngine>();
            _mockCareAdviceBuilder = new Mock<ICareAdviceBuilder>();
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _mockCacheManager = new Mock<ICacheManager<string, string>>();
            _mockNotifier = new Mock<INotifier<string>>();
            _mockItkMessagingFeature = new Mock<IITKMessagingFeature>();

            SetupMockFillCareAdviceBuilder();

            SetupMockConfiguration();

            _dosBuilder = new DOSBuilder(_mockCareAdviceBuilder.Object, 
                _mockRestfulHelper.Object, 
                _mockConfiguration.Object, 
                _mappingEngine.Object, _mockCacheManager.Object,
                _mockNotifier.Object,
                _mockItkMessagingFeature.Object);
        }

        private void SetupMockConfiguration()
        {
            _expectedBusinessApiPathwaySymptomGroupUrl = "http://Test.ApiPathwaySymptomGroupUrl.com/" + _mockPathwayURL;
            _mockConfiguration.Setup(c => c.GetBusinessApiPathwaySymptomGroupUrl(_mockPathwayURL)).Returns(_expectedBusinessApiPathwaySymptomGroupUrl);
            _mockConfiguration.Setup(c => c.DOSWhitelist).Returns("Service 1|Service 2");
        }

        private void SetupMockFillCareAdviceBuilder()
        {
            var mockCareAdvices = new List<CareAdvice>()
            {
                new CareAdvice() {Title = "TestAdvice", Id = "CA123", Items = new List<CareAdviceText>() {new CareAdviceText(){Text = "Test advice text"}}}
            };


            _mockCareAdviceBuilder.Setup(
                cb =>
                    cb.FillCareAdviceBuilder(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<List<string>>())).ReturnsAsync(mockCareAdvices);
        }

        [Test]
        public async void FillCheckCapacitySummaryResult_WithDistanceInMetric_ConvertsToMiles() {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent("{CheckCapacitySummaryResult: [{}]}")
            };
            _mockRestfulHelper.Setup(r => r.PostAsync(It.IsAny<string>(), It.IsAny<HttpRequestMessage>()))
                .Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));
            _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>())).Returns(Task<string>.Factory.StartNew(() => "0"));

            var model = new DosViewModel {
                SearchDistance = 1,
                DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult()
                {
                    Success = new SuccessObject<ServiceViewModel>()
                    {
                        Services = new List<ServiceViewModel>()
                    }
                } 
            };
            await _dosBuilder.FillCheckCapacitySummaryResult(model, true, null);

            _mockRestfulHelper.Verify(r => r.PostAsync(It.IsAny<string>(), It.Is<HttpRequestMessage>(h => AssertIsMetric(h, model.SearchDistance))));
        }

        [Test]
        public void FillGroupedDosServices_WithEmptyList_ReturnsEmptyList()
        {
            var emptyServiceList = new List<ServiceViewModel>();
            var groupedDosServices = _dosBuilder.FillGroupedDosServices(emptyServiceList);
            Assert.IsEmpty(groupedDosServices);
        }

        [Test]
        public void FillGroupedDosServices_WithSingleService_ReturnsIteminList()
        {
            var emptyServiceList =
                new List<ServiceViewModel>() {new ServiceViewModel() {CallbackEnabled = true, Id = 1}};
            var groupedDosServices = _dosBuilder.FillGroupedDosServices(emptyServiceList);
            Assert.IsTrue(groupedDosServices.Count == 1);
            Assert.AreEqual(OnlineDOSServiceType.Callback, groupedDosServices.FirstOrDefault().OnlineDOSServiceType);
            Assert.IsTrue(groupedDosServices.FirstOrDefault().Services.Count() ==1);
            Assert.AreEqual(1, groupedDosServices.FirstOrDefault().Services.FirstOrDefault().Id);
        }

        [Test]
        public void FillGroupedDosServices_WithMixedServices_ReturnsGroupedItemsinList()
        {
            var emptyServiceList =
                new List<ServiceViewModel>()
                {
                    new ServiceViewModel() { CallbackEnabled = true, Id = 1 },
                    new ServiceViewModel() { CallbackEnabled = true, Id = 2 },
                    new ServiceViewModel() { CallbackEnabled = false, Id = 3, ContactDetails = "02380123456"},
                    new ServiceViewModel() { CallbackEnabled = false, Id = 4, }
                };
            var groupedDosServices = _dosBuilder.FillGroupedDosServices(emptyServiceList);
            Assert.IsTrue(groupedDosServices.Count == 3);
            Assert.AreEqual(OnlineDOSServiceType.Callback, groupedDosServices[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, groupedDosServices[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, groupedDosServices[2].OnlineDOSServiceType);

            Assert.AreEqual(2, groupedDosServices[0].Services.Count());
            Assert.AreEqual(1, groupedDosServices[1].Services.Count());
            Assert.AreEqual(1, groupedDosServices[2].Services.Count());

            Assert.IsTrue(groupedDosServices[0].Services.Any(s => s.Id == 2));
            Assert.IsTrue(groupedDosServices[1].Services.All(s => s.Id == 3));
            Assert.IsTrue(groupedDosServices[2].Services.All(s => s.Id == 4));
        }

        [Test]
        public void FillGroupedDosServices_WithOrderedMixedServices_ReturnsGroupedItemsinCorrectOrder()
        {
            var emptyServiceList =
                new List<ServiceViewModel>()
                {
                    new ServiceViewModel() { CallbackEnabled = false, Id = 1, ContactDetails = "02380123456"},
                    new ServiceViewModel() { CallbackEnabled = true, Id = 2 },
                    new ServiceViewModel() { CallbackEnabled = true, Id = 3 },
                    new ServiceViewModel() { CallbackEnabled = false, Id = 4, ContactDetails = "02380123456"},
                    new ServiceViewModel() { CallbackEnabled = false, Id = 5, },
                    new ServiceViewModel() { CallbackEnabled = true, Id = 6 },
                    new ServiceViewModel() { CallbackEnabled = false, Id = 7, }
                };
            var groupedDosServices = _dosBuilder.FillGroupedDosServices(emptyServiceList);
            Assert.IsTrue(groupedDosServices.Count == 3);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, groupedDosServices[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Callback, groupedDosServices[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, groupedDosServices[2].OnlineDOSServiceType);

            Assert.AreEqual(2, groupedDosServices[0].Services.Count());
            Assert.AreEqual(3, groupedDosServices[1].Services.Count());
            Assert.AreEqual(2, groupedDosServices[2].Services.Count());

            Assert.AreEqual(1, groupedDosServices[0].Services.ToList()[0].Id);
            Assert.AreEqual(4, groupedDosServices[0].Services.ToList()[1].Id);
            Assert.AreEqual(2, groupedDosServices[1].Services.ToList()[0].Id);
            Assert.AreEqual(3, groupedDosServices[1].Services.ToList()[1].Id);
            Assert.AreEqual(6, groupedDosServices[1].Services.ToList()[2].Id);
            Assert.AreEqual(5, groupedDosServices[2].Services.ToList()[0].Id);
            Assert.AreEqual(7, groupedDosServices[2].Services.ToList()[1].Id);
        }


        private bool AssertIsMetric(HttpRequestMessage request, int original) {
            var content = request.Content.ReadAsStringAsync().Result;
            var payload = JsonConvert.DeserializeObject<DosCase>(content);

            const float MILES_PER_KM = 1.609344f;
            return payload.SearchDistance == (int)Math.Ceiling(original / MILES_PER_KM);
        }

        private void MockRestfulHelperWithExpectedUrl(string expectedSymptomGroup)
        {
            _mockRestfulHelper.Setup(r => r.GetAsync(_expectedBusinessApiPathwaySymptomGroupUrl)).ReturnsAsync(expectedSymptomGroup);
        }

    }
}
