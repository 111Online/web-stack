using NHS111.Domain.IoC;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Domain.Api.IoC
{
    public class DomainApiRegistry : Registry
    {
        public DomainApiRegistry()
        {
            IncludeRegistry<DomainRegistry>();
            IncludeRegistry<UtilsRegistry>();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}