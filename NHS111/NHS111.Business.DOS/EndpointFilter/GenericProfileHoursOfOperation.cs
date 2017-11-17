using NodaTime;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class GenericProfileHoursOfOperation : ProfileHoursOfOperation
    {
        public GenericProfileHoursOfOperation(LocalTime workingDayInHoursStartTime,
            LocalTime workingDayInHoursShoulderEndTime,
            LocalTime workingDayInHoursEndTime)
            : base(workingDayInHoursStartTime, workingDayInHoursShoulderEndTime, workingDayInHoursEndTime)
        {
        }
    }
}
