using log4net;
using NHS111.Utils.Storage;
using StructureMap;

namespace NHS111.Utils.IoC
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            For<ILog>().Use(LogManager.GetLogger("log"));
            //For<IRestfulHelper>().Use<RestfulHelper>().SelectConstructor(() => new RestfulHelper());
            For<IStorageService>().Use<AzureStorageService>().Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}