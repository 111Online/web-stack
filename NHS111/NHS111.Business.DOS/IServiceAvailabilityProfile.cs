using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS
{
    public interface IServiceAvailabilityProfile
    {
        ServiceAvailability GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes);
    }
}
