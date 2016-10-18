using System;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using DayOfWeek = System.DayOfWeek;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class ServiceViewModelTests
    {
        private readonly ServiceCareItemRotaSession MONDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession TUESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession WEDNESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession THURSDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession FRIDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession SATURDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 0},
            EndTime = new TimeOfDay() {Hours = 18, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession SUNDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            StartTime = new TimeOfDay() {Hours = 10, Minutes = 0},
            EndTime = new TimeOfDay() {Hours = 16, Minutes = 0}
        };

        [Test]
        public void IsOpen_Returns_True_When_Service_Open_All_Hours()
        {
            var service = new ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.IsTrue(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_True_When_Service_Is_Open()
        {
            var clock = new StaticClock(DayOfWeek.Monday, 10, 37);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.IsTrue(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Is_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Sunday, 16, 2);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.IsFalse(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Has_No_Rota_Sessions()
        {
            var clock = new StaticClock(DayOfWeek.Sunday, 16, 2);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
            };

            Assert.IsFalse(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Has_No_Rota_Session_For_Today()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            Assert.IsFalse(service.IsOpen);
        }

        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Open today: 09:00 until 18:00", service.CurrentStatus);
        }

        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times_When_Time_Before_Todays_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 7, 30);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Open today: 09:30 until 17:00", service.CurrentStatus);
        }

        [Test]
        public void CurrentStatus_Returns_Closed_When_Time_After_Todays_Closing()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 19, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }

        [Test]
        public void CurrentStatus_Returns_Closed_When_No_Rota_Session_For_Today()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }
    }

    public class StaticClock : IClock
    {
        private readonly DayOfWeek _day;
        private readonly int _hour;
        private readonly int _minute;
        private readonly DateTime _date;

        public StaticClock(DayOfWeek day, int hour, int minute)
        {
            _day = day;
            _hour = hour;
            _minute = minute;

            //get the next date that the given day of week falls on
            var start = (int)DateTime.Now.DayOfWeek;
            var target = (int)_day;
            if (target <= start)
                target += 7;
            _date = DateTime.Now.AddDays(target - start);
        }

        public DateTime Now
        {
            get
            {
                return new DateTime(_date.Year, _date.Month, _date.Day, _hour, _minute, 0);
            }
        }
    }
}
