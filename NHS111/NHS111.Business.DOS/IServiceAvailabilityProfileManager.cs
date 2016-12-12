using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHS111.Business.DOS
{
    public interface IServiceAvailabilityProfileManager
    {
        IServiceAvailabilityProfile FindServiceAvailability(int dxCode);
    }
}
