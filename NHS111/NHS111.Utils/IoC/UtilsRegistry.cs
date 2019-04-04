using System;
using System.Configuration;
using log4net;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using NHS111.Utils.Storage;
using RestSharp;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace NHS111.Utils.IoC
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            For<ILog>().Use(LogManager.GetLogger("log"));
            For<IRestfulHelper>().Use<RestfulHelper>().SelectConstructor(() => new RestfulHelper());
            For<IStorageService>().Use<AzureStorageService>().SelectConstructor(() => new AzureStorageService());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}