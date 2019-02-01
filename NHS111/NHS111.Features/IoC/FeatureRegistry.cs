using NHS111.Models.Models.Web.Clock;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Features.IoC
{
    public class FeatureRegistry : Registry
    {
        public FeatureRegistry()
        {
            For<IDOSSpecifyDispoTimeFeature>().Use(new DOSSpecifyDispoTimeFeature(new SystemClock()));
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
