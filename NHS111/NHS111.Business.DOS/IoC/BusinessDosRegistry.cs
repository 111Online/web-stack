using System.Net;
using log4net;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.DispositionMapper;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Business.DOS.Service;
using NHS111.Business.DOS.WhiteListPopulator;
using NHS111.Features;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.Clock;
using NHS111.Utils.IoC;
using NHS111.Utils.Logging;
using NHS111.Utils.RestTools;
using RestSharp;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.DOS.IoC
{
    public class BusinessDosRegistry : Registry
    {
        private IConfiguration _configuration;
        private ILog _logger;

        public BusinessDosRegistry(IConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;

            IncludeRegistry<UtilsRegistry>();
            For<IServiceAvailabilityManager>().Use<ServiceAvailablityManager>();
            For<IRestClient>().Singleton()
                .Use<IRestClient>(GetLoggingRestClientFor(configuration.DomainDosApiBaseUrl));
            For<ISearchDistanceService>().Singleton()
                .Use<SearchDistanceService>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(configuration.CCGApiBaseUrl));
            For<IWhiteListManager>().Singleton()
                .Use<WhiteListManager>()
                .Ctor<IRestClient>()
                .Is(GetLoggingRestClientFor(configuration.CCGApiBaseUrl));
            For<IPublicHolidayService>().Use(new PublicHolidayService(
                PublicHolidaysDataService.GetPublicHolidays(configuration),
                new SystemClock()));
            For<IFilterServicesFeature>().Use<FilterServicesFeature>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private LoggingRestClient GetLoggingRestClientFor(string baseUrl)
        {
            return new LoggingRestClient(baseUrl, _logger, _configuration.ServicePointManagerDefaultConnectionLimit);
        }
    }
}
