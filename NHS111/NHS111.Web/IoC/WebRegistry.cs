

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
            Configure();
        }

        public WebRegistry(IConfiguration configuration) {
            For<ICacheManager<string, string>>().Use(new RedisManager(configuration.RedisConnectionString));
            For<IRestClient>().Use(new LoggingRestClient(configuration.BusinessApiProtocolandDomain, LogManager.GetLogger("log"))).Named("restClientBusinessApi");
            Configure();
        }

        private void Configure() {
            IncludeRegistry<FeatureRegistry>();
            IncludeRegistry<UtilsRegistry>();
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry<WebPresentationRegistry>();
            For<INotifier<string>>().Use<Notifier>();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}