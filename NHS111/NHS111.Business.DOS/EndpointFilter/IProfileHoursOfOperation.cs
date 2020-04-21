using NHS111.Models.Models.Business;
using System;

namespace NHS111.Business.DOS.EndpointFilter
{
    public interface IProfileHoursOfOperation
    {
        ProfileServiceTimes GetServiceTime(DateTime date);

        bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime);
    }
}
