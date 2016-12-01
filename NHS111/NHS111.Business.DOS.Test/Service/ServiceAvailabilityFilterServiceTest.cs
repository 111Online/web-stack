using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NodaTime;
using NUnit.Framework;

namespace NHS111.Business.DOS.Test.Service
{
    public class ServiceAvailabilityFilterServiceTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IDosService> _mockDosService;
        private const string DOS_USERNAME = "made_up_user";
        private const string DOS_PASSWORD = "made_up_password";
        private const string FILTERED_DISPOSITION_CODES = "1005|1006|1007|1008";
        private const string FILTERED_DOS_SERVICE_IDS = "100|25";

        private static readonly string CheckCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    }
                },
                {
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    }
                },
                {
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    }
                },
            ]}";

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _mockDosService = new Mock<IDosService>();

            _mockConfiguration.Setup(c => c.DosUsername).Returns(DOS_USERNAME);
            _mockConfiguration.Setup(c => c.DosPassword).Returns(DOS_PASSWORD);
            _mockConfiguration.Setup(c => c.FilteredDispositionCodes).Returns(FILTERED_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredDosServiceIds).Returns(FILTERED_DOS_SERVICE_IDS);
            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursEndTime).Returns(new LocalTime(18, 0));
            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursShoulderEndTime).Returns(new LocalTime(9, 0));
            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursStartTime).Returns(new LocalTime(8, 0));
        }

        [Test]
        public async void failed_request_should_return_empty_CheckCapacitySummaryResult()
        {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{CheckCapacitySummaryResult: [{}]}")
            };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1010 };
            var fakeRequest = new HttpRequestMessage() { Content = new StringContent(JsonConvert.SerializeObject(fakeDoSFilteredCase)) };

            _mockDosService.Setup(x => x.GetServices(It.IsAny<HttpRequestMessage>())).Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object);

            //Act
            var result = await sut.GetFilteredServices(fakeRequest);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<HttpRequestMessage>()), Times.Once);
            var JObj = GetJObjectFromResponse(result);
            var services = JObj["CheckCapacitySummaryResult"];
            Assert.AreEqual("{CheckCapacitySummaryResult: [{}]}", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async void non_filtered_disposition_should_return_unfiltered_CheckCapacitySummaryResult()
        {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(CheckCapacitySummaryResults)
            };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1010 };
            var fakeRequest = new HttpRequestMessage() { Content = new StringContent(JsonConvert.SerializeObject(fakeDoSFilteredCase)) };

            _mockDosService.Setup(x => x.GetServices(It.IsAny<HttpRequestMessage>())).Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object);

            //Act
            var result = await sut.GetFilteredServices(fakeRequest);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<HttpRequestMessage>()), Times.Once);

            var JObj = GetJObjectFromResponse(result);
            var services = JObj["CheckCapacitySummaryResult"];
            Assert.AreEqual(3, services.Count());
        }

        [Test]
        public async void in_hours_should_return_filtered_CheckCapacitySummaryResult()
        {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(CheckCapacitySummaryResults)
            };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1008, DispositionTime = new DateTime(2016, 11, 23, 9, 30, 0), DispositionTimeFrameMinutes = 60 };
            var request = new HttpRequestMessage() { Content = new StringContent(JsonConvert.SerializeObject(fakeDoSFilteredCase)) };

            _mockDosService.Setup(x => x.GetServices((It.IsAny<HttpRequestMessage>()))).Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object);

            //Act
            var result = await sut.GetFilteredServices(request);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<HttpRequestMessage>()), Times.Once);

            var jObj = GetJObjectFromResponse(result);
            var services = jObj["CheckCapacitySummaryResult"];
            Assert.AreEqual(1, services.Count());
        }

        [Test]
        public async void out_of_hours_should_return_unfiltered_CheckCapacitySummaryResult()
        {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(CheckCapacitySummaryResults)
            };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1008, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };
            var request = new HttpRequestMessage() { Content = new StringContent(JsonConvert.SerializeObject(fakeDoSFilteredCase)) };

            _mockDosService.Setup(x => x.GetServices((It.IsAny<HttpRequestMessage>()))).Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object);

            //Act
            var result = await sut.GetFilteredServices(request);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<HttpRequestMessage>()), Times.Once);

            var jObj = GetJObjectFromResponse(result);
            var services = jObj["CheckCapacitySummaryResult"];
            Assert.AreEqual(3, services.Count());
        }

        [Test]
        public async void in_hours_shoulder_should_return_filtered_CheckCapacitySummaryResult()
        {
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(CheckCapacitySummaryResults)
            };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1008, DispositionTime = new DateTime(2016, 11, 23, 8, 20, 0), DispositionTimeFrameMinutes = 720 };
            var request = new HttpRequestMessage() { Content = new StringContent(JsonConvert.SerializeObject(fakeDoSFilteredCase)) };

            _mockDosService.Setup(x => x.GetServices((It.IsAny<HttpRequestMessage>()))).Returns(Task<HttpResponseMessage>.Factory.StartNew(() => fakeResponse));

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object);

            //Act
            var result = await sut.GetFilteredServices(request);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<HttpRequestMessage>()), Times.Once);

            var jObj = GetJObjectFromResponse(result);
            var services = jObj["CheckCapacitySummaryResult"];
            Assert.AreEqual(1, services.Count());
        }

        private static JObject GetJObjectFromResponse(HttpResponseMessage response)
        {
            var val = response.Content.ReadAsStringAsync().Result;
            return (JObject)JsonConvert.DeserializeObject(val);
        }
    }
}
