using System.Net;

namespace NHS111.Domain.DOS.Api.Configuration
{
    public interface IConfiguration
    {
        string DOSIntegrationBaseUrl { get; }
        string DOSMobileIntegrationBaseUrl { get; }
        string DOSMobileIntegrationUsername { get; }
        string DOSMobileIntegrationPassword { get; }
        string DOSIntegrationCheckCapacitySummaryUrl { get; }
        string DOSIntegrationServiceDetailsByIdUrl { get; }
        string DOSIntegrationMonitorHealthUrl { get; }
        string DOSMobileIntegrationServicesByClinicalTermUrl { get; }
    }
}
