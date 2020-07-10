using NHS111.Features.Clock;
using StructureMap;

namespace NHS111.Features.IoC
{
    public class FeatureRegistry : Registry
    {
        public FeatureRegistry()
        {
            For<IDOSSpecifyDispoTimeFeature>().Use(new DOSSpecifyDispoTimeFeature(new SystemClock()));
            For<ISurveyLinkFeature>().Use(new SurveyLinkFeature());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
