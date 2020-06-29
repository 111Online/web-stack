using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using DayOfWeek = System.DayOfWeek;
using TimeOfDay = NHS111.Models.Models.Web.FromExternalServices.TimeOfDay;

namespace NHS111.Models.Models.Web
{
    public class ServiceViewModel : Business.DosService
    {
        private readonly IEnumerable<long> _callbackCASIdList = new List<long> { 130, 133, 137, 138 };

        private readonly IClock _clock;
        private const string ServiceClosedMessage = "Closed";
        private const string OpenAllHoursMessage = "Open today: 24 hours";

        private List<string> _addressLines;

        public ServiceViewModel()
            : this(new SystemClock())
        {
        }

        public ServiceViewModel(IClock clock) : base(clock)
        {
            _clock = clock;
        }

        public List<string> AddressLines
        {
            get { return _addressLines ?? (_addressLines = BuildFormattedAddressLines(WebUtility.HtmlDecode(this.Address))); }
        }

        private List<string> BuildFormattedAddressLines(string unformattedAddress)
        {
            return String.IsNullOrEmpty(unformattedAddress)
                ? new List<string>()
                : unformattedAddress.Split(',').ToList()
                    .Select(a => IsAPostcode(a) ? a.Trim() : TitleCaseAddressLine(a)).ToList();
        }

        private string TitleCaseAddressLine(string addressLine)
        {
            return new CultureInfo("en-US", false).TextInfo
                .ToTitleCase(addressLine.ToLower()).Trim();
        }

        private bool IsAPostcode(string addressLine)
        {
            var postcodeRegex = new Regex(PostCodeFormatValidator.PostcodeRegex, RegexOptions.IgnoreCase);
            return postcodeRegex.IsMatch(addressLine.Replace(" ", ""));
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

        private string GetOpeningTimes(DayOfWeek day)
        {
            if (RotaSessionsAndSpecifiedSessions == null) return ServiceClosedMessage;

            var rotaSession = RotaSessionsAndSpecifiedSessions.FirstOrDefault(rs => (int)rs.StartDayOfWeek == (int)day);
            return rotaSession != null
                ? string.Format("{0} - {1}", GetTime(rotaSession.StartTime), GetTime(rotaSession.EndTime))
                : ServiceClosedMessage;
        }

        private IEnumerable<RotaSession> GetRotaSessions(DayOfWeek day)
        {
            return RotaSessionsAndSpecifiedSessions.Where(rs => (int)rs.StartDayOfWeek == (int)day).Select(rs => new RotaSession()
            {
                OpeningTime = new TimeSpan(rs.StartTime.Hours, rs.StartTime.Minutes, 0),
                ClosingTime = new TimeSpan(rs.EndTime.Hours, rs.EndTime.Minutes, 0),
                Day = (DayOfWeek)rs.StartDayOfWeek,
            });
        }

        private string GetTime(TimeOfDay time)
        {
            return DateTime.Today.Add(new TimeSpan(time.Hours, time.Minutes, 0)).ToString("h:mmtt").ToLower();
        }

        public string NextOpeningTimePrefixMessage
        {
            get
            {
                if (ServiceClosed || OpenAllHours) return "";
                var rotaSession = CurrentRotaSession;
                string openingTense = (IsOpen) ? "Open" : "Opens";

                if (rotaSession == null) rotaSession = NextRotaSession;

                return String.Format("{0} {1}:",
                    openingTense,
                    GetDayMessage(rotaSession.Day));
            }
        }

        public string ServiceOpeningTimesMessage
        {
            get
            {
                if (OpenAllHours) return OpenAllHoursMessage;

                if (ServiceClosed) return ServiceClosedMessage;

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

        public bool ServiceClosed
        {
            get { return (RotaSessionsAndSpecifiedSessions == null || !RotaSessionsAndSpecifiedSessions.Any()); }
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


        public IEnumerable<RotaSession> NextOpenDayRotaSessions
        {
            get
            {
                if (OpenAllHours || ServiceClosed) return new List<RotaSession>();
                return GetRotaSessions(NextRotaSession.Day).ToList();
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

                var sessionsForDay = RotaSessionsAndSpecifiedSessions.Where(rs => (int)rs.StartDayOfWeek == dayToCheck).OrderBy(rs => new TimeSpan(rs.StartTime.Hours, rs.StartTime.Minutes, 0));

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

        public RotaSession CurrentRotaSession
        {
            get
            {
                return TodaysRotaSessions
                    .OrderBy(rs => rs.OpeningTime)
                    .FirstOrDefault(rs => _clock.Now.TimeOfDay < rs.ClosingTime);
            }
        }

        public bool IsCASServiceType
        {
            get { return _callbackCASIdList.Contains(ServiceType.Id); }
        }
    }

    public class RotaSession
    {
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public DayOfWeek Day { get; set; }
    }

}
