using NHS111.Domain.IoC;
using NHS111.Utils.IoC;
using StructureMap;

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