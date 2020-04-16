using NHS111.Business.IoC;
using NHS111.Models.IoC;
using NHS111.Utils.Cache;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Business.Api.IoC
{
    public class BusinessApiRegistry : Registry
    {
        public BusinessApiRegistry()
        {
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry<UtilsRegistry>();
            IncludeRegistry(new BusinessRegistry(new Configuration.Configuration()));
            For<ICacheManager<string, string>>().Use(new RedisManager(new Configuration.Configuration().GetRedisUrl()));
            For<ICacheStore>().Use<RedisCacheStore>()
                .Ctor<bool>().Is(true).Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}