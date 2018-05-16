using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using SimpleJson;
using DayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek;

namespace NHS111.Business.DOS.Service
{
    public interface IPublicHolidayService
    {
        IEnumerable<NHS111.Models.Models.Business.DosService> AdjustServiceRotaSessionOpeningForPublicHoliday(IEnumerable<NHS111.Models.Models.Business.DosService> services);
    }

    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly IPublicHolidayData _publicHolidayData;
        private readonly IClock _clock;
        public PublicHolidayService(IPublicHolidayData publicHolidayData, IClock clock)
        {
            _publicHolidayData = publicHolidayData;
            _clock = clock;
        }

        public IEnumerable<NHS111.Models.Models.Business.DosService> AdjustServiceRotaSessionOpeningForPublicHoliday(IEnumerable<NHS111.Models.Models.Business.DosService> services)
        {
            if(ContainsBankHolidaySessions(services)) return services;

            var ajustedServices = new List<NHS111.Models.Models.Business.DosService>();
            foreach (var service in services)
            {
                var bankHoldaySession = service.RotaSessions.FirstOrDefault(s => s.StartDayOfWeek == DayOfWeek.BankHoliday);
                var ajustedSessions = new List<ServiceCareItemRotaSession>();
                    foreach (var session in service.RotaSessions)
                    {
                        var ajsutedSession = session;
                        if (bankHoldaySession !=null)
                        {
                            DateTime sessionDate =
                                _clock.Now.AddDays(NumberOfDaysBetweenWeekdays(session.StartDayOfWeek, _clock.Now.DayOfWeek));
                            if (_publicHolidayData != null && _publicHolidayData.PublicHolidays.Any(h => h.Date.Date == sessionDate.Date))
                            {
                                
                                ajsutedSession = new ServiceCareItemRotaSession()
                                {
                                    StartDayOfWeek = session.StartDayOfWeek,
                                    StartTime = bankHoldaySession.StartTime,
                                    EndDayOfWeek = session.EndDayOfWeek,
                                    EndTime = bankHoldaySession.EndTime,
                                    Status = bankHoldaySession.Status
                                };
                            }
                             
                        }
                        ajustedSessions.Add(ajsutedSession);
                    }

                service.RotaSessions = ajustedSessions.ToArray();
                ajustedServices.Add(service);
            }

            return ajustedServices;
        }

        private int NumberOfDaysBetweenWeekdays(DayOfWeek futureDay, System.DayOfWeek startDay)
        {
            return (7 + (int)futureDay - (int)startDay) % 7;
        }

        private bool ContainsBankHolidaySessions(IEnumerable<Models.Models.Business.DosService> services)
        {

            return !services.Any(s => s.RotaSessions.Any(rs => rs.StartDayOfWeek == DayOfWeek.BankHoliday || rs.EndDayOfWeek == DayOfWeek.BankHoliday));
        }
    }

    public interface IPublicHolidayData
    {
        IEnumerable<PublicHoliday> PublicHolidays { get; }

    }
}
