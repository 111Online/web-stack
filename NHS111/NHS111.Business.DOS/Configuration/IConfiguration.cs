using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NodaTime;

namespace NHS111.Business.DOS.Configuration
{
    public interface IConfiguration
    {
        LocalTime WorkingDayInHoursStartTime { get; }
        LocalTime WorkingDayInHoursEndTime { get; }
        LocalTime WorkingDayInHoursShoulderEndTime { get; }
        string DomainDOSApiBaseUrl { get; }
        string DomainDOSApiCheckCapacitySummaryUrl { get; }
        string DomainDOSApiServiceDetailsByIdUrl { get; }
        string DomainDOSApiMonitorHealthUrl { get; }
        string DomainDOSApiServicesByClinicalTermUrl { get; }
    }
}
