

using log4net;
using NHS111.Features.IoC;
using NHS111.Utils.Logging;
using NHS111.Utils.RestTools;
using NHS111.Web.Controllers;
using RestSharp;

namespace NHS111.Web.IoC {
    using Models.IoC;
    using Utils.Cache;
    using Utils.IoC;
    using Utils.Notifier;
    using Presentation.Configuration;
    using Presentation.IoC;
    using StructureMap;
    using StructureMap.Graph;

    public class WebRegistry : Registry {
        public WebRegistry() {
            Configure(new Configuration());
        }

        public WebRegistry(IConfiguration configuration) {
            For<ICacheManager<string, string>>().Use(new RedisManager(configuration.RedisConnectionString));
            For<IRestClient>().Use(new LoggingRestClient(configuration.BusinessApiProtocolandDomain, LogManager.GetLogger("log"))).Named("restClientBusinessApi");
            Configure(configuration);
        }

        private void Configure(IConfiguration configuration)
        {
            IncludeRegistry<FeatureRegistry>();
            IncludeRegistry<UtilsRegistry>();
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry(new WebPresentationRegistry(configuration));
            For<INotifier<string>>().Use<Notifier>();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}