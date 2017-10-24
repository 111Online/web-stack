using log4net;
using NHS111.Business.Configuration;
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
            IncludeRegistry<UtilsRegistry>();
            For<IRestClient>().Use(new LoggingRestClient(configuration.GetLocationBaseUrl(), LogManager.GetLogger("log"))).Named("restidealPostcodesApi");
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}