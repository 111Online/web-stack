using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Models.Web.ServiceViewModelTests
{
    [TestFixture]
    class OpeningTimesMessageTests
    {
        private readonly ServiceViewModelTestHelper _serviceViewModelTestHelper = new ServiceViewModelTestHelper();

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Tomorrow_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 12, 30);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.MONDAY_SESSION,
                    _serviceViewModelTestHelper.TUESDAY_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_SESSION,
                    _serviceViewModelTestHelper.FRIDAY_SESSION,
                    _serviceViewModelTestHelper.SATURDAY_SESSION,
                    _serviceViewModelTestHelper.SUNDAY_SESSION
                }
            };
            Assert.AreEqual("Opens tomorrow:", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Opens tomorrow: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }


        [Test]
        public void ServiceOpeningTimesMessage_Returns_Next_Day_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 12, 30);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.TUESDAY_SESSION
                }
            };
            Assert.AreEqual("Opens Tuesday:", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Opens Tuesday: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_First_Session_Of_Two_On_Same_Day_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Friday, 12, 30);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.THURSDAY_AFTERNOON_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_MORNING_SESSION
                }
            };
            Assert.AreEqual("Opens Thursday:", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Opens Thursday: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Correct_Open_Tomorrow_Times()
        {
            var clock = new StaticClock(DayOfWeek.Sunday, 22, 35);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.MONDAY_SESSION,
                    _serviceViewModelTestHelper.TUESDAY_SESSION,
                    _serviceViewModelTestHelper.WEDNESDAY_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_SESSION,
                    _serviceViewModelTestHelper.FRIDAY_SESSION,
                }
            };

            Assert.AreEqual("Opens tomorrow:", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Opens tomorrow: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Correct_Open_Day_After_Tomorrow_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 22, 35);
            var service = new NHS111.Models.Models.Web.ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    _serviceViewModelTestHelper.MONDAY_SESSION,
                    _serviceViewModelTestHelper.TUESDAY_SESSION,
                    _serviceViewModelTestHelper.WEDNESDAY_SESSION,
                    _serviceViewModelTestHelper.THURSDAY_SESSION,
                    _serviceViewModelTestHelper.FRIDAY_SESSION,
                }
            };

            Assert.AreEqual("Opens Monday:", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Opens Monday: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_24Hours_When_Service_Open_All_Hours()
        {
            var service = new NHS111.Models.Models.Web.ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.AreEqual("", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Open today: 24 hours", service.ServiceOpeningTimesMessage);
        }
        [Test]
        public void ServiceOpeningTimesMessage_Returns_Closed_When_Not_Open_All_Hours_And_No_Rotasessions()
        {
            var service = new NHS111.Models.Models.Web.ServiceViewModel()
            {
                OpenAllHours = false,
            };

            Assert.AreEqual("", service.NextOpeningTimePrefixMessage);
            Assert.AreEqual("Closed", service.ServiceOpeningTimesMessage);
        }
    }
}
