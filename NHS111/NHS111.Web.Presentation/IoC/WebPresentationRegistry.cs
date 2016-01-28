using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Web.Presentation.IoC
{
    public class WebPresentationRegistry : Registry
    {
        public WebPresentationRegistry()
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