using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Business.Feedback.Api.IoC
{
    public class FeedbackDomainApiRegistry : Registry
    {
        public FeedbackDomainApiRegistry()
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