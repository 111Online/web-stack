using AutoMapper;
using NHS111.Models.Mappers.WebMappings;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Models.IoC
{
    public class ModelsRegistry : Registry
    {
        public ModelsRegistry()
        {
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