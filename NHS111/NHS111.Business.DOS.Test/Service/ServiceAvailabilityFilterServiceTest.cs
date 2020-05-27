using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.DispositionMapper;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Business.DOS.Service;
using NHS111.Business.DOS.WhiteListPopulator;
using NHS111.Features;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHS111.Business.DOS.Test.Service
{
    public class ServiceAvailabilityFilterServiceTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IDosService> _mockDosService;
        private Mock<IServiceAvailabilityManager> _mockServiceAvailabilityProfileManager;
        private Mock<IFilterServicesFeature> _mockFilterServicesFeature;
        private Mock<IOnlineServiceTypeFilter> _mockServiceTypeFilter;
        private Mock<IPublicHolidayService> _mockPublicHolidayService;
        private Mock<ISearchDistanceService> _mocSearchDistanceService;
        private Mock<IWhiteListManager> _mockWhiteListManager;
        private IDispositionMapper _dispositionMapper;

        private const string DOS_USERNAME = "made_up_user";
        private const string DOS_PASSWORD = "made_up_password";
        private const string FILTERED_DISPOSITION_CODES = "1005|1006|1007|1008";
        private const string FILTERED_DOS_SERVICE_IDS = "100|25";

        private const string FILTERED_DENTAL_DISPOSITION_CODES = "1017|1018|1019|1020|1021|1022";
        private const string FILTERED_DENTAL_DOS_SERVICE_IDS = "100|123|117|40|25|12";

        private const string FILTERED_CLINICIAN_DISPOSITION_CODES = "11329|11106|1034|11327|11325|1035|1032";
        private const string FILTERED_CLINICIAN_DOS_SERVICE_IDS = "40";

        private const string FILTERED_REPEAT_PRESCRIPTION_DISPOSITION_CODES = "1100|1101|1102|1103";
        private const string FILTERED_REPEAT_PRESCRIPTION_SERVICE_IDS = "100|40|25";

        private const string FILTERED_GENERIC_DISPOSITION_CODES = "1200|1201|1202|1203";
        private const string FILTERED_GENERIC_SERVICE_IDS = "100|40|25";

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
                }
            ]}";

        [SetUp]
        public void SetUp()
        {
            var workingDayPrimaryCareInHoursEndTime = new LocalTime(18, 0);
            var workingDayPrimaryCareInHoursShoulderEndTime = new LocalTime(9, 0);
            var workingDayPrimaryCareInHoursStartTime = new LocalTime(8, 0);

            var workingDayDentalInHoursEndTime = new LocalTime(22, 0);
            var workingDayDentalInHoursShoulderEndTime = new LocalTime(7, 30);
            var workingDayDentalInHoursStartTime = new LocalTime(7, 30);

            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _mockDosService = new Mock<IDosService>();
            _mockServiceAvailabilityProfileManager = new Mock<IServiceAvailabilityManager>();
            _mockFilterServicesFeature = new Mock<IFilterServicesFeature>();
            _mockServiceTypeFilter = new Mock<IOnlineServiceTypeFilter>();

            _mockPublicHolidayService = new Mock<IPublicHolidayService>();

            _mocSearchDistanceService = new Mock<ISearchDistanceService>();

            _mockWhiteListManager = new Mock<IWhiteListManager>();

            _dispositionMapper = new DispositionMapper.DispositionMapper(_mockConfiguration.Object);


            _mockConfiguration.Setup(c => c.DosUsername).Returns(DOS_USERNAME);
            _mockConfiguration.Setup(c => c.DosPassword).Returns(DOS_PASSWORD);
            _mockConfiguration.Setup(c => c.FilteredPrimaryCareDispositionCodes).Returns(FILTERED_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredPrimaryCareDosServiceIds).Returns(FILTERED_DOS_SERVICE_IDS);
            _mockConfiguration.Setup(c => c.FilteredDentalDosServiceIds).Returns(FILTERED_DENTAL_DOS_SERVICE_IDS);
            _mockConfiguration.Setup(c => c.FilteredDentalDispositionCodes).Returns(FILTERED_DENTAL_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredClinicianCallbackDispositionCodes).Returns(FILTERED_CLINICIAN_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredClinicianCallbackDosServiceIds).Returns(FILTERED_CLINICIAN_DOS_SERVICE_IDS);

            _mockConfiguration.Setup(c => c.FilteredRepeatPrescriptionDispositionCodes).Returns(FILTERED_REPEAT_PRESCRIPTION_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredRepeatPrescriptionDosServiceIds).Returns(FILTERED_REPEAT_PRESCRIPTION_SERVICE_IDS);
            _mockConfiguration.Setup(c => c.FilteredGenericDispositionCodes).Returns(FILTERED_GENERIC_DISPOSITION_CODES);
            _mockConfiguration.Setup(c => c.FilteredGenericDosServiceIds).Returns(FILTERED_GENERIC_SERVICE_IDS);

            _mockConfiguration.Setup(c => c.WorkingDayPrimaryCareInHoursStartTime)
                .Returns(workingDayPrimaryCareInHoursStartTime);
            _mockConfiguration.Setup(c => c.WorkingDayPrimaryCareInHoursShoulderEndTime)
                .Returns(workingDayPrimaryCareInHoursShoulderEndTime);
            _mockConfiguration.Setup(c => c.WorkingDayPrimaryCareInHoursEndTime)
                .Returns(workingDayPrimaryCareInHoursEndTime);

            _mockConfiguration.Setup(c => c.WorkingDayDentalInHoursStartTime)
               .Returns(workingDayDentalInHoursStartTime);
            _mockConfiguration.Setup(c => c.WorkingDayDentalInHoursShoulderEndTime)
                .Returns(workingDayDentalInHoursShoulderEndTime);
            _mockConfiguration.Setup(c => c.WorkingDayDentalInHoursEndTime)
                .Returns(workingDayDentalInHoursEndTime);
        }

        [Test]
        public async void failed_request_should_return_empty_CheckCapacitySummaryResult()
        {
            var fakeResponse = new DosCheckCapacitySummaryResult() { Error = new ErrorObject { Message = "Failed", Code = 500 } };

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = 1010 };

            _mockDosService.Setup(x => x.GetServices(It.IsAny<DosCheckCapacitySummaryRequest>(), null)).Returns(Task<DosCheckCapacitySummaryResult>.Factory.StartNew(() => fakeResponse));

            _mockServiceAvailabilityProfileManager.Setup(c => c.FindServiceAvailability(fakeDoSFilteredCase))
                .Returns(new ServiceAvailability(null, fakeDoSFilteredCase.DispositionTime, fakeDoSFilteredCase.DispositionTimeFrameMinutes));

            _mockFilterServicesFeature.Setup(c => c.IsEnabled).Returns(true);

            var sut = new ServiceAvailabilityFilterService(_mockDosService.Object, _mockConfiguration.Object, _mockServiceAvailabilityProfileManager.Object, _mockFilterServicesFeature.Object, _mockServiceTypeFilter.Object, _mockPublicHolidayService.Object, _mocSearchDistanceService.Object, _mockWhiteListManager.Object);

            //Act
            var result = await sut.GetFilteredServices(fakeDoSFilteredCase, true, null);

            //Assert 
            _mockDosService.Verify(x => x.GetServices(It.IsAny<DosCheckCapacitySummaryRequest>(), null), Times.Once);
            Assert.AreEqual(fakeResponse.Error.Code, result.Error.Code);
        }

        [Test]
        public void non_filtered_disposition_should_return_unfiltered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1010;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode };

            //Act
            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void in_hours_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1008;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 9, 30, 0), DispositionTimeFrameMinutes = 60 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void out_of_hours_should_return_unfiltered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1008;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };


            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void in_hours_shoulder_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1008;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 8, 20, 0), DispositionTimeFrameMinutes = 720 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void in_hours_shoulder_on_the_button_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1008;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 8, 0, 0), DispositionTimeFrameMinutes = 1440 };


            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void out_of_hours_traversing_in_hours_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1008;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 12, 1, 18, 1, 0), DispositionTimeFrameMinutes = 1440 };


            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Dental_out_of_hours_traversing_in_hours_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1017;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 12, 1, 22, 1, 0), DispositionTimeFrameMinutes = 1440 };


            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Dental_in_hours_shoulder_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1017;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 7, 31, 0), DispositionTimeFrameMinutes = 720 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Dental_No_Blacklisted_Services_Returns_All_CheckCapacitySummaryResults()
        {
            _mockConfiguration.Setup(c => c.FilteredDentalDispositionCodes).Returns("");

            var dispoCode = 1017;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 7, 31, 0), DispositionTimeFrameMinutes = 720 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);

            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Dental_out_of_hours_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1017;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };


            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void RepeatPrescription_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1100;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Generic_should_return_filtered_CheckCapacitySummaryResult()
        {
            var dispoCode = 1200;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            //Assert 

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void ClinicianCallback_should_return_all_CheckCapacitySummaryResult()
        {
            var dispoCode = 1032;

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            var fakeDoSFilteredCase = new DosFilteredCase() { PostCode = "So30 2Un", Disposition = dispoCode, DispositionTime = new DateTime(2016, 11, 23, 23, 30, 0), DispositionTimeFrameMinutes = 60 };

            var sut = new ServiceAvailablityManager(_mockConfiguration.Object, _dispositionMapper).FindServiceAvailability(fakeDoSFilteredCase);
            //Act
            var result = sut.Filter(results);

            //Assert 
            Assert.AreEqual(3, result.Count());
        }
    }
}
