using NHS111.Integration.DOS.Api.DOSService;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Integration.DOS.Api.IoC
{
    public class IntegrationDosApiRegistry : Registry
    {
        public IntegrationDosApiRegistry()
        {
            IncludeRegistry<UtilsRegistry>();



            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}