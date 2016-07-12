using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Business.Glossary.Api.Services;
using NHS111.Domain.Glossary;
using NHS111.Domain.Glossary.IoC;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.Glossary.Api.IoC
{
    public class GlossaryBusinessApiRegistry : Registry
    {
        public GlossaryBusinessApiRegistry()
        {
            IncludeRegistry<GlossaryDomainRegistry>();
            IncludeRegistry<UtilsRegistry>();
            For<ITermsService>().Use<TermsService>().Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            
            
        }
    }
}