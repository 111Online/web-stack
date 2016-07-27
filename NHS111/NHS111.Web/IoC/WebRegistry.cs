

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
            Configure();
        }

        private void Configure() {
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