using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.IoC
{
    public class BusinessRegistry : Registry
    {
        public BusinessRegistry()
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