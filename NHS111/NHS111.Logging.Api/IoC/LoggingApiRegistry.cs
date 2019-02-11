using NHS111.Logging.Api.StorageProviders;
using NHS111.Models.IoC;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Logging.Api.IoC
{
    public class LoggingApiRegistry : Registry
    {
        public LoggingApiRegistry()
        {
            IncludeRegistry<UtilsRegistry>();
            For<ILogStorageProvider>().Use(new Log4NetStorageProvider());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}