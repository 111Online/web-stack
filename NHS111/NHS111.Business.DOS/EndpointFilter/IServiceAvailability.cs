using System.Collections.Generic;

namespace NHS111.Business.DOS.EndpointFilter
{
    public interface IServiceAvailability
    {
        bool IsOutOfHours { get; }

        List<Models.Models.Business.DosService> Filter(
            List<Models.Models.Business.DosService> resultsToFilter);
    }
}
