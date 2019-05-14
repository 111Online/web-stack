using System.Configuration;
using log4net;
using NHS111.Business.Configuration;
using NHS111.Business.Services;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using RestSharp;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.IoC
{
    public class BusinessRegistry : Registry
    {
        public BusinessRegistry(IConfiguration configuration)
        {
            For<IRestClient>().Singleton().Use<IRestClient>(new LoggingRestClient(configuration.GetDomainApiBaseUrl(), LogManager.GetLogger("log")));
            For<ICCGDetailsService>().Singleton()
                .Use<CCGDetailsService>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(configuration.GetCCGBaseUrl(), LogManager.GetLogger("log")));

            For<ISearchResultFilter>().Use<EmergencyPrescriptionResultFilter>().Ctor<ICCGDetailsService>();
            For<ICategoryFilter>().Use<EmergencyPrescriptionResultFilter>().Ctor<ICCGDetailsService>();

            For<ILocationService>().Singleton()
                .Use<LocationService>()
                .Ctor<IRestClient>()
                .Is(new LoggingRestClient(configuration.GetLocationBaseUrl(), LogManager.GetLogger("log")));
            IncludeRegistry<UtilsRegistry>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}