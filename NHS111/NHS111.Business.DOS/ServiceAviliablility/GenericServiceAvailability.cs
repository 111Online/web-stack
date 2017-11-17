using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Business.DOS.ServiceAviliablility
{
    public class GenericServiceAvailability : ServiceAvailability
    {
        public GenericServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile,
            DateTime dispositionDateTime, int dispostionTimeframe)
            : base(serviceAvailabilityProfile, dispositionDateTime, dispostionTimeframe)
        {

        }
    }
}
