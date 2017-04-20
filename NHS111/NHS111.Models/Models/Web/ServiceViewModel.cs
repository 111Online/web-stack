using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using DayOfWeek = System.DayOfWeek;

namespace NHS111.Models.Models.Web
{
    public class ServiceViewModel : DosService
    {
        private readonly IClock _clock;

        public ServiceViewModel() : this(new SystemClock())
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

                return !TodaysRotaSessions.All(c => _clock.Now.TimeOfDay >= c.ClosingTime);
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
            if (RotaSessions == null) return "Closed";

            var rotaSession = RotaSessions.FirstOrDefault(rs => (int) rs.StartDayOfWeek == (int) day);
            return rotaSession != null
                ? string.Format("{0} - {1}", GetTime(rotaSession.StartTime), GetTime(rotaSession.EndTime))
                : "Closed";
        }

        private string GetTime(TimeOfDay time)
        {
            return DateTime.Today.Add(new TimeSpan(time.Hours, time.Minutes, 0)).ToString("h:mmtt").ToLower();
        }

        public string CurrentStatus
        {
            get
            {
                if (!IsOpen) return "Closed";

                if (OpenAllHours) return "Open today: 24 hours";

                return CurrentRotaSession != null 
                    ? string.Format("Open today: {0} until {1}", DateTime.Today.Add(CurrentRotaSession.OpeningTime).ToString("HH:mm"), DateTime.Today.Add(CurrentRotaSession.ClosingTime).ToString("HH:mm"))
                    : "Closed";
            }
        }

        private IEnumerable<ServiceCareItemRotaSession> TodaysServiceCareItemRotaSessions
        {
            get
            {
                return RotaSessions != null &&
                       RotaSessions.Any(rs => (int) rs.StartDayOfWeek == (int) _clock.Now.DayOfWeek)
                    ? RotaSessions.Where(rs => (int) rs.StartDayOfWeek == (int) _clock.Now.DayOfWeek)
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
                                ClosingTime = new TimeSpan(rs.EndTime.Hours, rs.EndTime.Minutes, 0)
                            })
                    : new List<RotaSession>();
            }
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
    }
}
