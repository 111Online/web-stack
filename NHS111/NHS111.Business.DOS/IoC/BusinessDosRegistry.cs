using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.DOS.IoC
{
    public class BusinessDosRegistry : Registry
    {
        public BusinessDosRegistry()
        {
            IncludeRegistry<UtilsRegistry>();
            For<IServiceAvailabilityManager>().Use<ServiceAvailablityManager>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
