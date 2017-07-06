using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nest;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using DayOfWeek = System.DayOfWeek;
using TimeOfDay = NHS111.Models.Models.Web.FromExternalServices.TimeOfDay;

namespace NHS111.Models.Models.Web
{
    public class ServiceViewModel : DosService
    {
        private readonly IClock _clock;
        private const string ServiceClosedMessage = "Closed";
        private const string OpenAllHoursMessage = "Open today: 24 hours";

        public ServiceViewModel()
            : this(new SystemClock())
        {
        }

        public ServiceViewModel(IClock clock)
        {
            _clock = clock;
        }
        public bool IsOpen
        {
            get
            {

                if (OpenAllHours) return true;
                if (TodaysRotaSessions == null || !TodaysRotaSessions.Any()) return false;
                return TodaysRotaSessions.Any(c => TimeBetween(_clock.Now.TimeOfDay, c.OpeningTime, c.ClosingTime));
            }
        }

        private static bool TimeBetween(TimeSpan timeNow, TimeSpan openingTime, TimeSpan closingTime)
        {
            return (timeNow >= openingTime && timeNow < closingTime);
        }

        public bool IsOpenToday
        {
            get
            {
                if (OpenAllHours) return true;

                return !TodaysRotaSessions.All(c => _clock.Now.TimeOfDay > c.ClosingTime);
            }
        }

        public Dictionary<DayOfWeek, string> OpeningTimes
        {
            get
            {
                if (OpenAllHours) return new Dictionary<DayOfWeek, string>();

                var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();
                return daysOfWeek.ToDictionary(day => day, GetOpeningTimes);
            }
        }

        public bool CallbackEnabled { get; set; }

        private string GetOpeningTimes(DayOfWeek day)
        {
            if (RotaSessions == null) return ServiceClosedMessage;

            var rotaSession = RotaSessions.FirstOrDefault(rs => (int)rs.StartDayOfWeek == (int)day);
            return rotaSession != null
                ? string.Format("{0} - {1}", GetTime(rotaSession.StartTime), GetTime(rotaSession.EndTime))
                : ServiceClosedMessage;
        }

        private string GetTime(TimeOfDay time)
        {
            return DateTime.Today.Add(new TimeSpan(time.Hours, time.Minutes, 0)).ToString("h:mmtt").ToLower();
        }

        public string ServiceOpeningTimesMessage
        {
            get
            {
                if (OpenAllHours) return OpenAllHoursMessage;

                if (RotaSessions == null || !RotaSessions.Any()) return ServiceClosedMessage;

                var rotaSession = CurrentRotaSession;
                string openingTense = (IsOpen) ? "Open" : "Opens";

                if (rotaSession == null) rotaSession = NextRotaSession;

                return string.Format("{0} {1}: {2} until {3}",
                    openingTense,
                    GetDayMessage(rotaSession.Day),
                    DateTime.Today.Add(rotaSession.OpeningTime).ToString("HH:mm"),
                    DateTime.Today.Add(rotaSession.ClosingTime).ToString("HH:mm"));
            }
        }

        private string GetDayMessage(DayOfWeek day)
        {
            if (_clock.Now.DayOfWeek == day) return "today";
            if (_clock.Now.AddDays(1).DayOfWeek == day) return "tomorrow";
            return CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)day];
        }


        public string CurrentStatus
        {
            get
            {
                if (!IsOpenToday) return ServiceClosedMessage;

                if (OpenAllHours) return OpenAllHoursMessage;

                return CurrentRotaSession != null
                    ? ServiceOpeningTimesMessage
                    : ServiceClosedMessage;
            }
        }

        private IEnumerable<ServiceCareItemRotaSession> TodaysServiceCareItemRotaSessions
        {
            get
            {
                return RotaSessions != null &&
                       RotaSessions.Any(rs => (int)rs.StartDayOfWeek == (int)_clock.Now.DayOfWeek)
                    ? RotaSessions.Where(rs => (int)rs.StartDayOfWeek == (int)_clock.Now.DayOfWeek)
                    : new List<ServiceCareItemRotaSession>();
            }
        }

        private IEnumerable<RotaSession> TodaysRotaSessions
        {
            get
            {
                return TodaysServiceCareItemRotaSessions.Any()
                    ? TodaysServiceCareItemRotaSessions.Select(
                        rs =>
                            new RotaSession
                            {
                                OpeningTime = new TimeSpan(rs.StartTime.Hours, rs.StartTime.Minutes, 0),
                                ClosingTime = new TimeSpan(rs.EndTime.Hours, rs.EndTime.Minutes, 0),
                                Day = (DayOfWeek)rs.StartDayOfWeek,
                            })
                    : new List<RotaSession>();
            }
        }

        private RotaSession NextRotaSession
        {
            get
            {
                var closedTime = _clock.Now;

                if (CurrentRotaSession != null)
                {
                    return CurrentRotaSession;
                }
                else
                {
                    ServiceCareItemRotaSession nextSession = GetNextDayServiceOpens((int)closedTime.DayOfWeek);
                    return new RotaSession() { Day = (DayOfWeek)nextSession.StartDayOfWeek, OpeningTime = new TimeSpan(nextSession.StartTime.Hours, nextSession.StartTime.Minutes, 0), ClosingTime = new TimeSpan(nextSession.EndTime.Hours, nextSession.EndTime.Minutes, 0) };
                }

            }
        }

        private ServiceCareItemRotaSession GetNextDayServiceOpens(int closedDay)
        {
            int daysCounted = 0;
            var dayToCheck = closedDay;

            do
            {
                dayToCheck = IncrementDayOfWeek(dayToCheck);

                var sessionsForDay = RotaSessions.Where(rs => (int)rs.StartDayOfWeek == dayToCheck).OrderBy(rs => new TimeSpan(rs.StartTime.Hours, rs.StartTime.Minutes, 0));

                if (sessionsForDay.Any())
                    return sessionsForDay.FirstOrDefault();

                daysCounted++;
            }
            while (daysCounted < 7);
            
            return new ServiceCareItemRotaSession();
        }

        private static int IncrementDayOfWeek(int dayOfWeek)
        {
            if (dayOfWeek > 5) return 0;
            return dayOfWeek + 1;
        }

        private RotaSession CurrentRotaSession
        {
            get
            {
                return TodaysRotaSessions
                    .OrderBy(rs => rs.OpeningTime)
                    .FirstOrDefault(rs => _clock.Now.TimeOfDay < rs.ClosingTime);
            }
        }
    }

    internal class RotaSession
    {
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public DayOfWeek Day { get; set; }
    }

}
