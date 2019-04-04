using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Web.Presentation.Builders.Tests
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Converters;
    using Models;
    using Newtonsoft.Json;
    using NHS111.Models.Mappers.WebMappings;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;

    [TestFixture()]
    public class DOSBuilderTests
    {

        private Mock<IMappingEngine> _mappingEngine;
        private Mock<ICareAdviceBuilder> _mockCareAdviceBuilder;
        private Mock<IRestClient> _mockRestClient;
        private Mock<Presentation.Configuration.IConfiguration> _mockConfiguration;
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
            _mockRestClient = new Mock<IRestClient>();
            _mockConfiguration = new Mock<Presentation.Configuration.IConfiguration>();
            _mockItkMessagingFeature = new Mock<IITKMessagingFeature>();

            SetupMockFillCareAdviceBuilder();

            SetupMockConfiguration();

            _dosBuilder = new DOSBuilder(_mockCareAdviceBuilder.Object,
                _mockRestClient.Object, 
                _mockConfiguration.Object, 
                _mappingEngine.Object,
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
        public async void FillCheckCapacitySummaryResult_WithDistanceInMetric_ConvertsToMiles()
        {
            var fakeContent = "{DosCheckCapacitySummaryResult: [{}]}";
            _mockRestClient.Setup(r => r.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(It.Is<IRestRequest>(rq => rq.Method == Method.POST)))
                .ReturnsAsync(new RestResponse<DosCheckCapacitySummaryResult> { Content = fakeContent, Data = JsonConvert.DeserializeObject<DosCheckCapacitySummaryResult>(fakeContent), ResponseStatus = ResponseStatus.Completed });
            _mockRestClient.Setup(r => r.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(It.Is<IRestRequest>(rq => rq.Method == Method.GET)))
                .ReturnsAsync(new RestResponse<DosCheckCapacitySummaryResult> { Content = "0", Data = JsonConvert.DeserializeObject<DosCheckCapacitySummaryResult>("0"), ResponseStatus = ResponseStatus.Completed });

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

            _mockRestClient.Verify(r => r.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(It.Is<RestRequest>(h => AssertIsMetric(h, model.SearchDistance))));
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
                new List<ServiceViewModel>() {new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 1}};
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
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 1 },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 2 },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone, Id = 3, ContactDetails = "02380123456"},
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.GoTo, Id = 4, }
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
        public void BuildDosViewModel_WithConfiguredDx_RemapsDxCode() {
            Mapper.Initialize(m => m.AddProfile<FromOutcomeViewModelToDosViewModel>());
            var model = new OutcomeViewModel {
                Id = "Dx01121",
                SymptomDiscriminatorCode = "1",
                UserInfo = new UserInfo {
                    Demography = new AgeGenderViewModel {
                        Gender = "Male"
                    }
                }
            };

            ConfigurationManager.AppSettings["Cat3And4DxCodes"] = "Dx01121";
            var dosModel = _dosBuilder.BuildDosViewModel(model, null);
            Assert.AreEqual(11333, dosModel.Disposition);
            ConfigurationManager.AppSettings["Cat3And4DxCodes"] = "";
            ConfigurationManager.AppSettings["EDCallbackDxCodes"] = "Dx01121";
            dosModel = _dosBuilder.BuildDosViewModel(model, null);
            Assert.AreEqual(11334, dosModel.Disposition);
        }

        [Test]
        public void FillGroupedDosServices_WithOrderedMixedServices_ReturnsGroupedItemsinCorrectOrder()
        {
            var emptyServiceList =
                new List<ServiceViewModel>()
                {
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone, Id = 1, ContactDetails = "02380123456"},
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 2 },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 3 },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone, Id = 4, ContactDetails = "02380123456"},
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.GoTo, Id = 5, },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.Callback, Id = 6 },
                    new ServiceViewModel() { OnlineDOSServiceType = OnlineDOSServiceType.GoTo, Id = 7, }
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


        private bool AssertIsMetric(RestRequest request, int original)
        {
            var content = request.Parameters.First(p => p.Type == ParameterType.RequestBody).Value;
            var payload = JsonConvert.DeserializeObject<DosCase>(content.ToString());

            const float MILES_PER_KM = 1.609344f;
            return payload.SearchDistance == (int)Math.Ceiling(original / MILES_PER_KM);
        }    
    }
}
