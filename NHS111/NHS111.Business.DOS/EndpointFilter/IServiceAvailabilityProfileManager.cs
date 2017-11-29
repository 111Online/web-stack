using NHS111.Models.Models.Web.DosRequests;

namespace NHS111.Business.DOS.EndpointFilter
{
    public interface IServiceAvailabilityManager
    {
        IServiceAvailability FindServiceAvailability(DosFilteredCase dosFilteredCase);
    }
}
