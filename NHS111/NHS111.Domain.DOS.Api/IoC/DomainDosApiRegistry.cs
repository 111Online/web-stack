using log4net;
using NHS111.Utils.Helpers;
using NHS111.Utils.IoC;
using NHS111.Utils.RestTools;
using RestSharp;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Domain.DOS.Api.IoC
{
    public class DomainDosApiRegistry : Registry
    {
        public DomainDosApiRegistry()
        {
            For<IRestClient>().Singleton().Use<IRestClient>(new LoggingRestClient(new Configuration.Configuration().DOSIntegrationBaseUrl, LogManager.GetLogger("log")));
            IncludeRegistry<UtilsRegistry>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}