using NHS111.Utils.Helpers;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Domain.DOS.Api.IoC
{
    public class DomainDosApiRegistry : Registry
    {
        public DomainDosApiRegistry()
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