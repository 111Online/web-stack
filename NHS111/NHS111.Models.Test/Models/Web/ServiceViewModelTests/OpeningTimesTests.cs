using System;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.ServiceViewModelTests
{
    [TestFixture]
    class OpeningTimesTests
    {
        private readonly ServiceViewModelTestHelper _serviceViewModelTestHelper = new ServiceViewModelTestHelper();

        [Test]
        public void OpeningTimes_Returns_List_Of_Opening_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.MONDAY_SESSION,
                    _serviceViewModelTestHelper.TUESDAY_SESSION,
                    _serviceViewModelTestHelper.WEDNESDAY_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_SESSION,
                    _serviceViewModelTestHelper.FRIDAY_SESSION
                }
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(7, openingTimes.Count);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Monday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Tuesday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Wednesday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Thursday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Friday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Saturday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Sunday]);
        }

        [Test]
        public void OpeningTimes_For_Open_All_Hours_Returns_Empty_List()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = true,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.MONDAY_SESSION,
                    _serviceViewModelTestHelper.TUESDAY_SESSION,
                    _serviceViewModelTestHelper.WEDNESDAY_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_SESSION,
                    _serviceViewModelTestHelper.FRIDAY_SESSION
                }
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(0, openingTimes.Count);
        }

        [Test]
        public void OpeningTimes_Null_Rota_Sessions_Returns_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(7, openingTimes.Count);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Monday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Tuesday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Wednesday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Thursday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Friday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Saturday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Sunday]);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_After_All_Returns_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 18, 00);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_Before_Morning_Returns_Morning_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 4, 00);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Opens today: 08:30 until 11:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_At_End_Of_First_Session_Returns_Morning()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 10, 59);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Open today: 08:30 until 11:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_Between_Sessions_Returns_Afternoon_Next_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 11, 00);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Opens today: 13:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 13:00 until 18:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_InBetween_Morning_Session_Returns_Morning_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 10, 00);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Open today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
            Assert.AreEqual("Open today: 08:30 until 11:00", service.CurrentStatus);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_InBetween_Afternoon_Session_Returns_Afternoon_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 15, 00);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Open today: 13:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 13:00 until 18:00", service.ServiceOpeningTimesMessage);
        }
    }
}
