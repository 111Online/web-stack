﻿using log4net;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.IoC;
using NHS111.Models.IoC;
using NHS111.Utils.Helpers;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Business.DOS.Api.IoC
{
    public class BusinessDosApiRegistry : Registry
    {
        public BusinessDosApiRegistry(IConfiguration configuration)
        {
            IncludeRegistry<ModelsRegistry>();
            IncludeRegistry(new BusinessDosRegistry(configuration, LogManager.GetLogger("log")));
            IncludeRegistry<UtilsRegistry>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}