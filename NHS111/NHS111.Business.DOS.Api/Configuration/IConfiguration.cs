namespace NHS111.Business.DOS.Api.Configuration
{
    public interface IConfiguration
    {
        string DomainDOSApiBaseUrl { get; }
        string DomainDOSApiCheckCapacitySummaryUrl { get; }
        string DomainDOSApiServiceDetailsByIdUrl { get; }
        string DomainDOSApiMonitorHealthUrl { get; }
        string DomainDOSApiServicesByClinicalTermUrl { get; }
        
    }
}
