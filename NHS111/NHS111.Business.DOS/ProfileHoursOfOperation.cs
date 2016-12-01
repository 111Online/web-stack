using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Business;
using NHS111.Utils.Dates;

namespace NHS111.Business.DOS
{
    public class ProfileHoursOfOperation : IProfileHoursOfOperation
    {
        private readonly IConfiguration _configuration;

        public ProfileHoursOfOperation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ProfileServiceTimes GetServiceTime(DateTime date)
        {
            if(IsNonWorkingDay(date)) return ProfileServiceTimes.OutOfHours;

            if (date.Hour < _configuration.WorkingDayInHoursStartTime.Hour) return ProfileServiceTimes.OutOfHours;

            if (date.Hour < _configuration.WorkingDayInHoursShoulderEndTime.Hour) return ProfileServiceTimes.InHoursShoulder;

            return date.Hour < _configuration.WorkingDayInHoursEndTime.Hour ? ProfileServiceTimes.InHours : ProfileServiceTimes.OutOfHours;
        }

        public bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            var days = GetListOfDatesInPeriod(startDateTime, endDateTime);
            if (days.TrueForAll(IsNonWorkingDay)) return false;

            var workingDays = days.Where(d => !IsNonWorkingDay(d));


            return workingDays.Any(
                d =>
                    OverlapsInHoursPeriod(startDateTime, endDateTime, GetInHoursStartDateTime(d),
                        GetInHoursEndDateTime(d)));
        }

        private static bool OverlapsInHoursPeriod(DateTime startDateTime, DateTime endDateTime, DateTime inHoursStartDateTime, DateTime inHoursEndDateTime)
        {
            return startDateTime < inHoursEndDateTime && inHoursStartDateTime < endDateTime;
        }

        private static List<DateTime> GetListOfDatesInPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            var days = new List<DateTime>();
            for (var date = startDateTime; date.Date <= endDateTime.Date; date = date.AddDays(1))
            {
                days.Add(date);
            }
            return days;
        }

        private static bool IsNonWorkingDay(DateTime date)
        {
            var dt = date.Date;
            return NonWorkingDays.IsWeekend(dt) || NonWorkingDays.IsBankHoliday(dt);
        }

        private DateTime GetInHoursStartDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, _configuration.WorkingDayInHoursStartTime.Hour, _configuration.WorkingDayInHoursStartTime.Minute, _configuration.WorkingDayInHoursStartTime.Second);
        }

        private DateTime GetInHoursEndDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, _configuration.WorkingDayInHoursEndTime.Hour, _configuration.WorkingDayInHoursEndTime.Minute, _configuration.WorkingDayInHoursEndTime.Second);
        }
    }
}
