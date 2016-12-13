using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHS111.Business.DOS.ServiceAviliablility;
using NHS111.Models.Models.Web.DosRequests;

namespace NHS111.Business.DOS
{
    public interface IServiceAvailabilityManager
    {
        IServiceAvailability FindServiceAvailability(DosFilteredCase dosFilteredCase);
    }
}
