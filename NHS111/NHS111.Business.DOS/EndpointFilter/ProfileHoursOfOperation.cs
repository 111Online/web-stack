using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Business;
using NHS111.Utils.Dates;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class ProfileHoursOfOperation : IProfileHoursOfOperation
    {

        private LocalTime _workingDayInHoursStartTime;
        private LocalTime _workingDayInHoursShoulderEndTime;
        private LocalTime _workingDayInHoursEndTime;
        private IConfiguration _configuration;
        private List<DateTime> _testHolidayDates;

        public ProfileHoursOfOperation(LocalTime workingDayInHoursStartTime, LocalTime workingDayInHoursShoulderEndTime,
            LocalTime workingDayInHoursEndTime, IConfiguration configuration)
        {
            _workingDayInHoursStartTime = workingDayInHoursStartTime;
            _workingDayInHoursShoulderEndTime = workingDayInHoursShoulderEndTime;
            _workingDayInHoursEndTime = workingDayInHoursEndTime;
            _configuration = configuration;
        }

        public virtual ProfileServiceTimes GetServiceTime(DateTime date)
        {
            if (IsNonWorkingDay(date)) return ProfileServiceTimes.OutOfHours;

            if (date.Hour < _workingDayInHoursStartTime.Hour) return ProfileServiceTimes.OutOfHours;

            if (date.Hour < _workingDayInHoursShoulderEndTime.Hour) return ProfileServiceTimes.InHoursShoulder;

            return date.Hour < _workingDayInHoursEndTime.Hour ? ProfileServiceTimes.InHours : ProfileServiceTimes.OutOfHours;
        }

        public virtual bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            var days = GetListOfDatesInPeriod(startDateTime, endDateTime);
            if (days.TrueForAll(IsNonWorkingDay)) return false;

            var workingDays = days.Where(d => !IsNonWorkingDay(d));


            return workingDays.Any(
                d =>
                    OverlapsInHoursPeriod(startDateTime, endDateTime, GetInHoursStartDateTime(d),
                        GetInHoursEndDateTime(d)));
        }

        protected static bool OverlapsInHoursPeriod(DateTime startDateTime, DateTime endDateTime, DateTime inHoursStartDateTime, DateTime inHoursEndDateTime)
        {
            return startDateTime < inHoursEndDateTime && inHoursStartDateTime < endDateTime;
        }

        protected static List<DateTime> GetListOfDatesInPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            var days = new List<DateTime>();
            for (var date = startDateTime; date.Date <= endDateTime.Date; date = date.AddDays(1))
            {
                days.Add(date);
            }
            return days;
        }

        protected bool IsNonWorkingDay(DateTime date)
        {
            var dt = date.Date;
            if (_testHolidayDates == null)
                _testHolidayDates = PublicHolidaysDataService.LoadTestHolidays(_configuration).Select(h => h.Date)
                    .ToList();
            return NonWorkingDays.IsWeekend(dt) || NonWorkingDays.IsBankHoliday(dt) ||
                   _testHolidayDates.Any(d => d.Date == dt.Date);
        }

        protected DateTime GetInHoursStartDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, _workingDayInHoursStartTime.Hour, _workingDayInHoursStartTime.Minute, _workingDayInHoursStartTime.Second);
        }

        protected DateTime GetInHoursEndDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, _workingDayInHoursEndTime.Hour, _workingDayInHoursEndTime.Minute, _workingDayInHoursEndTime.Second);
        }
    }
}
