using System.Net;

namespace NHS111.Domain.DOS.Api.Configuration
{
    public interface IConfiguration
    {
        string DOSIntegrationBaseUrl { get; }
        string DOSMobileBaseUrl { get; }
        string DOSMobileUsername { get; }
        string DOSMobilePassword { get; }
        string DOSIntegrationCheckCapacitySummaryUrl { get; }
        string DOSIntegrationServiceDetailsByIdUrl { get; }
        string DOSIntegrationMonitorHealthUrl { get; }
        string DOSMobileServicesByClinicalTermUrl { get; }
    }
}
