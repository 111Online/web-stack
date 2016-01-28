using NHS111.Domain.Repository;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Domain.IoC
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            IncludeRegistry<UtilsRegistry>();

            For<IGraphRepository>().Use<GraphRepository>().Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}