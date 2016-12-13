using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business;
using NodaTime;

namespace NHS111.Business.DOS.ServiceAviliablility
{
    public class DentalProfileHoursOfOperation : ProfileHoursOfOperation
    {
        private LocalTime _workingDayInHoursStartTime;
        private LocalTime _workingDayInHoursShoulderEndTime;
        private LocalTime _workingDayInHoursEndTime;
        public DentalProfileHoursOfOperation(LocalTime workingDayInHoursStartTime,
            LocalTime workingDayInHoursShoulderEndTime,
            LocalTime workingDayInHoursEndTime)
            : base(workingDayInHoursStartTime, workingDayInHoursShoulderEndTime, workingDayInHoursEndTime)
        {
            _workingDayInHoursStartTime = workingDayInHoursStartTime;
            _workingDayInHoursShoulderEndTime = workingDayInHoursShoulderEndTime;
            _workingDayInHoursEndTime = workingDayInHoursEndTime;
        }

        public override ProfileServiceTimes GetServiceTime(DateTime date)
        {
            if (date.Hour == _workingDayInHoursStartTime.Hour)
            {
                if(date.Minute > _workingDayInHoursStartTime.Minute) return ProfileServiceTimes.InHours;
            }
            if (date.Hour == _workingDayInHoursEndTime.Hour)
            {
                if (date.Minute < _workingDayInHoursEndTime.Minute) return ProfileServiceTimes.InHours;
            }

            if((date.Hour > _workingDayInHoursStartTime.Hour) &&
            (date.Hour < _workingDayInHoursEndTime.Hour)) return ProfileServiceTimes.InHours;

            return ProfileServiceTimes.OutOfHours;
        }

        public override bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            var days = GetListOfDatesInPeriod(startDateTime, endDateTime);

            return days.Any(
                d =>
                    OverlapsInHoursPeriod(startDateTime, endDateTime, GetInHoursStartDateTime(d),
                        GetInHoursEndDateTime(d)));
        }
    }
}
