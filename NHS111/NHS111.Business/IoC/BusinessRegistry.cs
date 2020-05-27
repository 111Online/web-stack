using log4net;
using NHS111.Business.Configuration;
using NHS111.Business.Services;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using RestSharp;
using StructureMap;

namespace NHS111.Business.IoC
{
    public class BusinessRegistry : Registry
    {
        private IConfiguration _configuration;

        public BusinessRegistry(IConfiguration configuration)
        {
            _configuration = configuration;

            For<ILoggingRestClient>().Singleton().Use<LoggingRestClient>(GetLoggingRestClientFor(configuration.GetDomainApiBaseUrl()));
            For<ICCGDetailsService>().Singleton()
                .Use<CCGDetailsService>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(configuration.GetCCGBaseUrl()));

            For<ISearchResultFilter>().Use<EmergencyPrescriptionResultFilter>().Ctor<ICCGDetailsService>();
            For<ICategoryFilter>().Use<EmergencyPrescriptionResultFilter>().Ctor<ICCGDetailsService>();

            For<ILocationService>().Singleton()
                .Use<LocationService>()
                .Ctor<ILoggingRestClient>()
                .Is(GetLoggingRestClientFor(configuration.GetLocationBaseUrl()));
            IncludeRegistry<UtilsRegistry>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

        private LoggingRestClient GetLoggingRestClientFor(string baseUrl)
        {
            return new LoggingRestClient(baseUrl, LogManager.GetLogger("log"), _configuration.GetServicePointManagerDefaultConnectionLimit(), _configuration.GetRestClientTimeoutMs());
        }
    }
}