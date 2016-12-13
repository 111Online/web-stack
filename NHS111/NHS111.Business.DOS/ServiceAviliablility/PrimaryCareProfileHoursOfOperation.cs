using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace NHS111.Business.DOS.ServiceAviliablility
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
