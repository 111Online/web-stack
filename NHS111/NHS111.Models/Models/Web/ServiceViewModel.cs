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

                return (TodaysOpeningTime <= _clock.Now.TimeOfDay) && (TodaysClosingTime >= _clock.Now.TimeOfDay);
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
            return rotaSession != null ? string.Format("{0} - {1}", GetTime(rotaSession.StartTime), GetTime(rotaSession.EndTime)) : "Closed";
        }

        private string GetTime(TimeOfDay time)
        {
            return DateTime.Today.Add(new TimeSpan(time.Hours, time.Minutes, 0)).ToString("h:mmtt").ToLower();
        }

        public string CurrentStatus
        {
            get
            {
                return _clock.Now.TimeOfDay > TodaysClosingTime ? "Closed" : string.Format("Open today: {0} until {1}", DateTime.Today.Add(TodaysOpeningTime).ToString("HH:mm"), DateTime.Today.Add(TodaysClosingTime).ToString("HH:mm"));
            }
        }

        private ServiceCareItemRotaSession TodaysRotaSession
        {
            get { return RotaSessions != null ? RotaSessions.FirstOrDefault(rs => (int)rs.StartDayOfWeek == (int)_clock.Now.DayOfWeek) : null; }
        }

        private TimeSpan TodaysOpeningTime
        {
            get { return TodaysRotaSession != null ? new TimeSpan(TodaysRotaSession.StartTime.Hours, TodaysRotaSession.StartTime.Minutes, 0) : new TimeSpan(); }
        }

        private TimeSpan TodaysClosingTime
        {
            get { return TodaysRotaSession != null ? new TimeSpan(TodaysRotaSession.EndTime.Hours, TodaysRotaSession.EndTime.Minutes, 0) : new TimeSpan(); }
        }

    }
}
