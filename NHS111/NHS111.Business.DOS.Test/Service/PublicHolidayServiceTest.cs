using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.Clock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek;

namespace NHS111.Business.DOS.Test.Service
{
    public class PublicHolidayServiceTest
    {
        private static readonly string CheckCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                       ""rotaSessionsField"": [
                            {
                                ""startDayOfWeekField"": 0,
                                ""startTimeField"": {
                                    ""hoursField"": 10,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 0,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 17,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
                            {
                                ""startDayOfWeekField"": 1,
                                ""startTimeField"": {
                                    ""hoursField"": 9,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 1,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 20,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
                            {
                                ""startDayOfWeekField"": 7,
                                ""startTimeField"": 
                                {
                                    ""hoursField"": 14,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 7,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 18,
                                    ""minutesField"": 30,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
 {
                                ""startDayOfWeekField"": 7,
                                ""startTimeField"": 
                                {
                                    ""hoursField"": 7,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 7,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 12,
                                    ""minutesField"": 30,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
                            {
                                ""startDayOfWeekField"": 2,
                                ""startTimeField"": 
                                {
                                    ""hoursField"": 9,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 2,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 13,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            }]
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

        private Mock<IPublicHolidayData> _mockPublicHolidayData;
        private Mock<IClock> _mockClock;

        [SetUp]
        public void SetUp()
        {
            _mockPublicHolidayData = new Mock<IPublicHolidayData>();
            _mockPublicHolidayData.SetupGet(c => c.PublicHolidays).Returns(new List<PublicHoliday>()
            {
                new PublicHoliday() {Date = new DateTime(2018, 5, 7)}
            });

            _mockClock = new Mock<IClock>();
            _mockClock.SetupGet(c => c.Now).Returns(new DateTime(2018, 5, 4));

        }


        [Test]
        public void BankHoliday_rotasessions_update_rotaSessions()
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var sut = new PublicHolidayService(_mockPublicHolidayData.Object, _mockClock.Object);
            //Act
            var result = sut.AdjustServiceRotaSessionOpeningForPublicHoliday(results);

            //Assert 
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(4, result.First().RotaSessions.Count());
            var test = result.First().RotaSessions[1];
            Assert.AreEqual(DayOfWeek.Monday, test.StartDayOfWeek);
            Assert.AreEqual(14, test.StartTime.Hours);
            Assert.AreEqual(0, test.StartTime.Minutes);
            Assert.AreEqual(18, test.EndTime.Hours);
            Assert.AreEqual(30, test.EndTime.Minutes);
        }


        [Test]
        public void BankHoliday_rotasessions_null_publicHolidays_collection()
        {

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var sut = new PublicHolidayService(null, _mockClock.Object);
            var result = sut.AdjustServiceRotaSessionOpeningForPublicHoliday(results);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(3, result.First().RotaSessions.Count());
        }

        [Test]
        public void BankHoliday_rotasessions_adjusted_for_publicHolidays_split_session()
        {

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var sut = new PublicHolidayService(_mockPublicHolidayData.Object, _mockClock.Object);
            var result = sut.AdjustServiceRotaSessionOpeningForPublicHoliday(results);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(2, result.First().RotaSessions.Where(s => s.StartDayOfWeek == DayOfWeek.Monday).Count());
        }

        [Test]
        public void BankHoliday_rotasessions_adjusted_for_open_when_usually_closed()
        {
            string checkCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                       ""rotaSessionsField"": [
                            {
                                ""startDayOfWeekField"": 0,
                                ""startTimeField"": {
                                    ""hoursField"": 10,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 0,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 17,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
 {
                                ""startDayOfWeekField"": 7,
                                ""startTimeField"": 
                                {
                                    ""hoursField"": 7,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 7,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 12,
                                    ""minutesField"": 30,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            }
                          ]
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
            var jObj = (JObject)JsonConvert.DeserializeObject(checkCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var sut = new PublicHolidayService(_mockPublicHolidayData.Object, _mockClock.Object);
            var result = sut.AdjustServiceRotaSessionOpeningForPublicHoliday(results);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(1, result.First().RotaSessions.Where(s => s.StartDayOfWeek == DayOfWeek.Monday).Count());
        }

        [Test]
        public void BankHoliday_rotasessions_adjusted_for_closed_when_usually_open()
        {
            string checkCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                       ""rotaSessionsField"": [
                            {
                                ""startDayOfWeekField"": 0,
                                ""startTimeField"": {
                                    ""hoursField"": 10,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 0,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 17,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            },
 {
                                ""startDayOfWeekField"": 1,
                                ""startTimeField"": 
                                {
                                    ""hoursField"": 7,
                                    ""minutesField"": 0,
                                    ""PropertyChanged"": null
                                },
                                ""endDayOfWeekField"": 1,
                                ""endTimeField"": 
                                {
                                    ""hoursField"": 12,
                                    ""minutesField"": 30,
                                    ""PropertyChanged"": null
                                },
                                ""statusField"": ""Open"",
                                ""PropertyChanged"": null
                            }
                          ]
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
            var jObj = (JObject)JsonConvert.DeserializeObject(checkCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();


            var sut = new PublicHolidayService(_mockPublicHolidayData.Object, _mockClock.Object);
            var result = sut.AdjustServiceRotaSessionOpeningForPublicHoliday(results);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(1, result.First().RotaSessions.Count());
            Assert.IsFalse(result.First().RotaSessions.Any(s => s.StartDayOfWeek == DayOfWeek.Monday));


        }
    }
}
