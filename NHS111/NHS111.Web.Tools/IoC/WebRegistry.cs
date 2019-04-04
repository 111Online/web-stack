using NHS111.Models.IoC;
using NHS111.Utils.Cache;
using NHS111.Utils.IoC;
using NHS111.Web.Presentation.Configuration;
using NHS111.Web.Presentation.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Web.IoC
{
    public class WebRegistry : Registry
    {
        public WebRegistry(IConfiguration configuration)
        {
            IncludeRegistry<UtilsRegistry>();
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry<WebPresentationRegistry>();
            For<ICacheManager<string, string>>().Use(new RedisManager(configuration.RedisConnectionString));
            
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}