using log4net;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Web.Presentation.IoC
{
    public class WebPresentationRegistry : Registry
    {
        public WebPresentationRegistry(IConfiguration configuration)
        {
            IncludeRegistry<UtilsRegistry>();
            For<IRestClient>().Use(new LoggingRestClient(configuration.BusinessApiProtocolandDomain, LogManager.GetLogger("log"))).Named("restLocationService");
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}