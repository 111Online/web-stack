using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Domain.Feedback.Repository;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Domain.Feedback.IoC
{
    public class FeedbackDomainRegistry : Registry
    {

        public FeedbackDomainRegistry()
        {
            IncludeRegistry<UtilsRegistry>();
            For<IFeedbackRepository>().Use<FeedbackRepository>().Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
