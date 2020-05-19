using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using DayOfWeek = System.DayOfWeek;

namespace NHS111.Models.Test.Models.Web.ServiceViewModelTests
{
    [TestFixture]
    class CurrentStatusTests
    {
        private readonly ServiceViewModelTestHelper _serviceViewModelTestHelper = new ServiceViewModelTestHelper();

        [Test]
        public void CurrentStatus_Returns_24Hours_When_Service_Open_All_Hours()
        {
            var service = new NHS111.Models.Models.Web.ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.AreEqual("Open today: 24 hours", service.CurrentStatus);
            Assert.AreEqual("Open today: 24 hours", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Open_Today_Midnight()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 6, 40);
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
                    new ServiceCareItemRotaSession()
                    {
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        StartTime = new TimeOfDay() { Hours = 0, Minutes = 0 },
                        EndTime = new TimeOfDay() { Hours = 7, Minutes = 30 }
                    },
                    new ServiceCareItemRotaSession()
                    {
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        StartTime = new TimeOfDay() { Hours = 22, Minutes = 0 },
                        EndTime = new TimeOfDay() { Hours = 23, Minutes = 59 }
                    },
                    _serviceViewModelTestHelper.SUNDAY_SESSION
                }
            };

            Assert.IsTrue(service.IsOpen);
            Assert.AreEqual("Open today: 00:00 until 07:30", service.CurrentStatus);
            Assert.AreEqual("Open today: 00:00 until 07:30", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 35);
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
                    _serviceViewModelTestHelper.SATURDAY_SESSION,
                    _serviceViewModelTestHelper.SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Open today: 09:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 09:00 until 18:00", service.ServiceOpeningTimesMessage);
        }



        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times_When_Time_Before_Todays_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 7, 30);
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
                    _serviceViewModelTestHelper.SATURDAY_SESSION,
                    _serviceViewModelTestHelper.SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Opens today: 09:30 until 17:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Closed_When_Time_After_Todays_Closing()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 19, 05);
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
                    _serviceViewModelTestHelper.SATURDAY_SESSION,
                    _serviceViewModelTestHelper.SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }

        [Test]
        public void CurrentStatus_Returns_Closed_When_No_Rota_Session_For_Today()
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

            Assert.AreEqual("Closed", service.CurrentStatus);
        }
    }
}
