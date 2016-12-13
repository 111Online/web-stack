using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Business.DOS.ServiceAviliablility
{
    public class PrimaryCareServiceAvailability : ServiceAvailability
    {
        public PrimaryCareServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile,
            DateTime dispositionDateTime, int dispostionTimeframe)
            : base(serviceAvailabilityProfile, dispositionDateTime, dispostionTimeframe)
        {
            
        }
    }
}
