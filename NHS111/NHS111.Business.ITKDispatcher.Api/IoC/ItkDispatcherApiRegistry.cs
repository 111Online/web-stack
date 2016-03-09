using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Business.ITKDispatcher.Api.Mappings;
using StructureMap;
using StructureMap.Graph;
using AutoMapperWebConfiguration = NHS111.Business.ITKDispatcher.Api.Mappings.AutoMapperWebConfiguration;

namespace NHS111.Business.ITKDispatcher.Api.IoC
{
    public class ItkDispatcherApiRegistry : Registry
    {
        public ItkDispatcherApiRegistry()
        {
            For<MessageEngine>().Use(new MessageEngineClient());
            For<IMappingEngine>().Use(() => Mapper.Engine);
            AutoMapperWebConfiguration.Configure();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}