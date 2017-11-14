using NodaTime;

namespace NHS111.Business.DOS.ServiceAvailability
{
    public class PrimaryCareProfileHoursOfOperation : ProfileHoursOfOperation
    {
        public PrimaryCareProfileHoursOfOperation(LocalTime workingDayInHoursStartTime,
            LocalTime workingDayInHoursShoulderEndTime,
            LocalTime workingDayInHoursEndTime)
            : base(workingDayInHoursStartTime, workingDayInHoursShoulderEndTime, workingDayInHoursEndTime)
        {
        }
    }
}
