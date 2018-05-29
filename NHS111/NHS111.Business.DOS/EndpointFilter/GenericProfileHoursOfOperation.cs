using NHS111.Business.DOS.Configuration;
using NodaTime;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class GenericProfileHoursOfOperation : ProfileHoursOfOperation
    {
        public GenericProfileHoursOfOperation(LocalTime workingDayInHoursStartTime,
            LocalTime workingDayInHoursShoulderEndTime,
            LocalTime workingDayInHoursEndTime, IConfiguration configuration)
            : base(workingDayInHoursStartTime, workingDayInHoursShoulderEndTime, workingDayInHoursEndTime, configuration)
        {
        }
    }
}
