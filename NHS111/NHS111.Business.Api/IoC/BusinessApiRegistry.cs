using NHS111.Business.IoC;
using NHS111.Models.IoC;
using NHS111.Utils.Cache;
using NHS111.Utils.IoC;
using StructureMap;

namespace NHS111.Business.Api.IoC
{
    public class BusinessApiRegistry : Registry
    {
        public BusinessApiRegistry()
        {
            var config = new Configuration.Configuration();
            
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry<UtilsRegistry>();
            IncludeRegistry(new BusinessRegistry(config));
            For<ICacheManager<string, string>>().Use(new RedisManager(config.GetRedisUrl(), config.GetRedisExpiryMinutes()));
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