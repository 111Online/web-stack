using System;
using System.Linq;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;

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

        //an array?
        public string OpeningTimes { get; }

        public string CurrentStatus
        {
            get
            {
                return _clock.Now.TimeOfDay > TodaysClosingTime ? "Closed" : string.Format("Open today: {0} until {1}", DateTime.Today.Add(TodaysOpeningTime).ToString("HH:mm").ToLower(), DateTime.Today.Add(TodaysClosingTime).ToString("HH:mm").ToLower());
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
