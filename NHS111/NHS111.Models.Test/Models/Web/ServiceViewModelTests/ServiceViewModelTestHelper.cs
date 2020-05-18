using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using System;
using DayOfWeek = System.DayOfWeek;

namespace NHS111.Models.Test.Models.Web.ServiceViewModelTests
{
    class ServiceViewModelTestHelper
    {
        internal readonly ServiceCareItemRotaSession MONDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 17, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession TUESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 17, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession WEDNESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 17, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession THURSDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 17, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession FRIDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 17, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession SATURDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            StartTime = new TimeOfDay() { Hours = 9, Minutes = 0 },
            EndTime = new TimeOfDay() { Hours = 18, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession SUNDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            StartTime = new TimeOfDay() { Hours = 10, Minutes = 0 },
            EndTime = new TimeOfDay() { Hours = 16, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession THURSDAY_MORNING_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() { Hours = 8, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 11, Minutes = 0 }
        };

        internal readonly ServiceCareItemRotaSession THURSDAY_AFTERNOON_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() { Hours = 13, Minutes = 00 },
            EndTime = new TimeOfDay() { Hours = 18, Minutes = 0 }
        };

        public bool IsSameTime(TimeSpan timespan, TimeOfDay rotaSessionTimeOfDay)
        {
            return timespan.Hours == rotaSessionTimeOfDay.Hours
                && timespan.Minutes == rotaSessionTimeOfDay.Minutes;
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
