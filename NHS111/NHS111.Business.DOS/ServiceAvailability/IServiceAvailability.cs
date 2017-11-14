using System.Collections.Generic;

namespace NHS111.Business.DOS.ServiceAvailability
{
    public interface IServiceAvailability
    {
        bool IsOutOfHours { get; }

        List<Models.Models.Web.FromExternalServices.DosService> Filter(
            List<Models.Models.Web.FromExternalServices.DosService> resultsToFilter);
    }
}
