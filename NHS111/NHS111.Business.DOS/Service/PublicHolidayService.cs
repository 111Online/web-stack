using AutoMapper.Internal;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
//using SimpleJson;
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
            var adjustedServices = new List<NHS111.Models.Models.Business.DosService>();
            foreach (var service in services)
            {
                var bankHolidaySessions = service.RotaSessions != null ? service.RotaSessions.Where(s => s.StartDayOfWeek == DayOfWeek.BankHoliday) : new List<ServiceCareItemRotaSession>();
                var adjustedSessions = new List<ServiceCareItemRotaSession>();

                adjustedSessions = GetAdjustedPublicHolidaySessions(service);
                service.RotaSessions = adjustedSessions.ToArray();
                adjustedServices.Add(service);
            }

            return adjustedServices;
        }


        private List<ServiceCareItemRotaSession> GetAdjustedPublicHolidaySessions(Models.Models.Business.DosService service)
        {
            var adjustedSessions = new List<ServiceCareItemRotaSession>();
            var bankHolidaySessions = service.RotaSessions != null ? service.RotaSessions.Where(s => s.StartDayOfWeek == DayOfWeek.BankHoliday) : new List<ServiceCareItemRotaSession>();
            if (bankHolidaySessions.Any())
            {
                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                {
                    if (dayOfWeek == DayOfWeek.BankHoliday) continue;
                    var standardScheduledRotaSessions = service.RotaSessions.Where(s => s.StartDayOfWeek == dayOfWeek);

                    DateTime dayOfWeekDate = _clock.Now.AddDays(NumberOfDaysBetweenWeekdays(dayOfWeek, _clock.Now.DayOfWeek));

                    if ((_publicHolidayData != null &&
                        _publicHolidayData.PublicHolidays.Any(h => h.Date.Date == dayOfWeekDate.Date)))
                    {
                        bankHolidaySessions.Each(s => adjustedSessions.Add(
                                new ServiceCareItemRotaSession()
                                {
                                    StartDayOfWeek = dayOfWeek,
                                    StartTime = s.StartTime,
                                    EndDayOfWeek = dayOfWeek,
                                    EndTime = s.EndTime,
                                    Status = s.Status
                                }
                            )
                        );

                    }
                    else
                    {
                        adjustedSessions.AddRange(standardScheduledRotaSessions);
                    }

                }
            }
            else
            {
                return RemovePublicHolidaySessions(service.RotaSessions).ToList();
            }

            return adjustedSessions;
        }


        private IEnumerable<ServiceCareItemRotaSession> RemovePublicHolidaySessions(IEnumerable<ServiceCareItemRotaSession> sessions)
        {
            return sessions != null ? sessions.Where(s => !_publicHolidayData.PublicHolidays.Any(ph =>
                 ph.Date == _clock.Now.AddDays(NumberOfDaysBetweenWeekdays(s.StartDayOfWeek, _clock.Now.DayOfWeek)).Date)) : new List<ServiceCareItemRotaSession>();
        }


        private int NumberOfDaysBetweenWeekdays(DayOfWeek futureDay, System.DayOfWeek startDay)
        {
            return (7 + (int)futureDay - (int)startDay) % 7;
        }

    }

    public interface IPublicHolidayData
    {
        IEnumerable<PublicHoliday> PublicHolidays { get; }

    }
}
