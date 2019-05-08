using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Clock;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class DosService
    {
        private readonly IClock _clock;

        public DosService()
            : this(new SystemClock())
        {
        }
        public DosService(IClock clock)
        {
            _clock = clock;
        }

        [JsonProperty(PropertyName = "idField")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "nameField")]
        public string Name { get; set; }

        private string _publicName;

        [JsonProperty(PropertyName = "publicNameField")]
        public string PublicName
        {
            get { return string.IsNullOrEmpty(_publicName) ? Name: _publicName; }
            set { _publicName = !string.IsNullOrEmpty(value) ? value : Name; }
        }

        [JsonProperty(PropertyName = "postcodeField")]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "odsCodeField")]
        public string OdsCode { get; set; }

        [JsonProperty(PropertyName = "capacityField")]
        public DosCapacity Capacity { get; set; }

        [JsonProperty(PropertyName = "contactDetailsField")]
        public string ContactDetails { get; set; }

        [JsonProperty(PropertyName = "addressField")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "northingsField")]
        public int Northings { get; set; }

        [JsonProperty(PropertyName = "eastingsField")]
        public int Eastings { get; set; }

        [JsonProperty(PropertyName = "urlField")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "serviceTypeField")]
        public ServiceType ServiceType { get; set; }

        [JsonProperty(PropertyName = "nonPublicTelephoneNo")]
        public string NonPublicTelephoneNo { get; set; }

        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "referralTextField")]
        public string ReferralText { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }

        [JsonProperty(PropertyName = "notesField")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "openAllHoursField")]
        public bool OpenAllHours { get; set; }

        private ServiceCareItemRotaSession[] _rotaSessions = new ServiceCareItemRotaSession[0];
        
        [JsonProperty(PropertyName = "rotaSessionsField")]
        public ServiceCareItemRotaSession[] RotaSessions {
            get
            {
                return _rotaSessions;
            }
            set
            {
                _rotaSessionsAndSpecifiedSessions = CombineRotaSessionsAndSpecifiedSessions(value, _openTimeSpecifiedSessions);
                _rotaSessions = value;
            }
        }

        private ServiceCareItemRotaSession[] _rotaSessionsAndSpecifiedSessions = new ServiceCareItemRotaSession[0];

        public ServiceCareItemRotaSession[] RotaSessionsAndSpecifiedSessions
        {
            get
            {
                if (_rotaSessionsAndSpecifiedSessions != null && _rotaSessionsAndSpecifiedSessions.Length != 0)
                    return _rotaSessionsAndSpecifiedSessions;

                return _rotaSessions;
            }
        }

        private string[] _openTimeSpecifiedSessions = new string[0];

        [JsonProperty(PropertyName = "openTimeSpecifiedSessionsField")]
        public string[] OpenTimeSpecifiedSessions
        {
            get { return _openTimeSpecifiedSessions; }
            set
            {
                _rotaSessionsAndSpecifiedSessions = CombineRotaSessionsAndSpecifiedSessions(_rotaSessions, value);
                _openTimeSpecifiedSessions = value;
            }
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
        public bool IsOpenForSpecifiedTimes
        {
            get
            {
                return DateFallsWithinSpecifiedOpeningTimes(OpenTimeSpecifiedSessions, _clock.Now);
            }
        }

        protected IEnumerable<RotaSession> TodaysRotaSessions
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
                                Day = (System.DayOfWeek)rs.StartDayOfWeek,
                            })
                    : new List<RotaSession>();
            }
        }

        private IEnumerable<ServiceCareItemRotaSession> TodaysServiceCareItemRotaSessions
        {
            get
            {
                return RotaSessionsAndSpecifiedSessions != null &&
                       RotaSessionsAndSpecifiedSessions.Any(rs => (int)rs.StartDayOfWeek == (int)_clock.Now.DayOfWeek)
                    ? RotaSessionsAndSpecifiedSessions.Where(rs => (int)rs.StartDayOfWeek == (int)_clock.Now.DayOfWeek)
                    : new List<ServiceCareItemRotaSession>();
            }
        }
        private static bool TimeBetween(TimeSpan timeNow, TimeSpan openingTime, TimeSpan closingTime)
        {
            return (timeNow >= openingTime && timeNow < closingTime);
        }


        private List<DateTime> GetWeeksDates(DateTime startDate)
        {
            List<DateTime> list = new List<DateTime>();

            for (var i = 1; i < 8; i++)
            {
                list.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            return list;
        }

        private bool IsDateInList(DateTime date, List<DateTime> nextWeekDates)
        {
            return nextWeekDates.Any(d => date.Date == d.Date);
        }

        private DateTime ConvertOpenTimeSpecifiedSessionToDate(string session)
        {
            try
            {
                var day = int.Parse(session.Substring(0, 2));
                var month = int.Parse(session.Substring(3, 2));
                var year = int.Parse(session.Substring(6, 4));

                return new DateTime(year, month, day);
            }
            catch (FormatException)
            {
                return new DateTime();
            }
        }

        private ServiceCareItemRotaSession[] CombineRotaSessionsAndSpecifiedSessions(ServiceCareItemRotaSession[] rotaSessions, string[] openTimeSpecifiedSessions)
        {
            if (openTimeSpecifiedSessions == null || openTimeSpecifiedSessions.Length == 0)
                return rotaSessions;

            var sessionsList = new List<ServiceCareItemRotaSession>();

            if (rotaSessions != null)
                sessionsList.AddRange(rotaSessions);

            var nextWeeksDates = GetWeeksDates(_clock.Now);

            var specifiedSessionsToAdd = new List<ServiceCareItemRotaSession>();
            var daysOfWeekToRemove = new List<DayOfWeek>();

            foreach (var session in openTimeSpecifiedSessions)
            {
                if (session.Length != 22)
                    continue;

                var date = ConvertOpenTimeSpecifiedSessionToDate(session);
;                if (!IsDateInList(date, nextWeeksDates))
                    continue;
                
                var dayOfWeek = Mapper.Map<DayOfWeek>(date.DayOfWeek);
                
                short startTimeHours;
                short startTimeMinutes;

                short endTimeHours;
                short endTimeMinutes;

                try
                {
                    startTimeHours = short.Parse(session.Substring(11, 2));
                    startTimeMinutes = short.Parse(session.Substring(14, 2));

                    endTimeHours = short.Parse(session.Substring(17, 2));
                    endTimeMinutes = short.Parse(session.Substring(20, 2));
                }
                catch (FormatException)
                {
                    continue;
                }
                
                ServiceCareItemRotaSession rotaSession =
                    new ServiceCareItemRotaSession
                    {
                        StartDayOfWeek = dayOfWeek,
                        EndDayOfWeek = dayOfWeek,
                        StartTime = new TimeOfDay
                        {
                            Hours = startTimeHours,
                            Minutes = startTimeMinutes
                        },
                        EndTime = new TimeOfDay
                        {
                            Hours = endTimeHours,
                            Minutes = endTimeMinutes
                        },
                        Status = "Open"
                    };

                daysOfWeekToRemove.Add(dayOfWeek);
                specifiedSessionsToAdd.Add(rotaSession);
            }

            foreach (var day in daysOfWeekToRemove)
            {
                sessionsList.RemoveAll(s => s.StartDayOfWeek.Equals(day));
            }

            sessionsList.AddRange(specifiedSessionsToAdd);

            return sessionsList.ToArray();
        }

        public bool DateFallsWithinSpecifiedOpeningTimes(string[] openTimeSpecifiedSessions, DateTime dateToFind)
        {
            foreach (var session in openTimeSpecifiedSessions)
            {
                if (session.Length != 22)
                    continue;
                var date = ConvertOpenTimeSpecifiedSessionToDate(session);
                if(dateToFind.Date != date.Date)
                    continue;

                int startTimeHours;
                int startTimeMinutes;

                int endTimeHours;
                int endTimeMinutes;

                try
                {
                    startTimeHours = int.Parse(session.Substring(11, 2));
                    startTimeMinutes = int.Parse(session.Substring(14, 2));

                    endTimeHours = int.Parse(session.Substring(17, 2));
                    endTimeMinutes = int.Parse(session.Substring(20, 2));
                }
                catch (FormatException)
                {
                    continue;
                }

                if (TimeBetween(dateToFind.TimeOfDay, new TimeSpan(startTimeHours, startTimeMinutes, 0), new TimeSpan(endTimeHours, endTimeMinutes, 0)))
                    return true;

            }
            return false;
        }
    }

   
    

    public enum DosCapacity
    {
        /// <remarks/>
        High,
        /// <remarks/>
        Low,
        /// <remarks/>
        None,
    }

    public class ServiceCareItemRotaSession
    {
        [JsonProperty(PropertyName = "startDayOfWeekField")]
        public DayOfWeek StartDayOfWeek { get; set; }
        [JsonProperty(PropertyName = "startTimeField")]
        public TimeOfDay StartTime { get; set; }
        [JsonProperty(PropertyName = "endDayOfWeekField")]
        public DayOfWeek EndDayOfWeek { get; set; }
        [JsonProperty(PropertyName = "endTimeField")]
        public TimeOfDay EndTime { get; set; }
        [JsonProperty(PropertyName = "statusField")]
        public string Status { get; set; }
    }

    public enum DayOfWeek
    {
        /// <remarks/>
        Sunday,
        /// <remarks/>
        Monday,
        /// <remarks/>
        Tuesday,
        /// <remarks/>
        Wednesday,
        /// <remarks/>
        Thursday,
        /// <remarks/>
        Friday,
        /// <remarks/>
        Saturday,

        BankHoliday
    }

    public class TimeOfDay
    {
        [JsonProperty(PropertyName = "hoursField")]
        public short Hours { get; set; }
        [JsonProperty(PropertyName = "minutesField")]
        public short Minutes { get; set; }
    }
}
