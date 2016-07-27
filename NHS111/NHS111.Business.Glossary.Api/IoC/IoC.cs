using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHS111.Domain.Glossary.IoC;
using StructureMap;

namespace NHS111.Business.Glossary.Api.IoC
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var cont = new Container(c => c.AddRegistry<GlossaryBusinessApiRegistry>());
           // cont.AssertConfigurationIsValid();
            return cont;
        }
    }
}