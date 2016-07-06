using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using NHS111.Business.Glossary.Api.Models;
using NHS111.Domain.Glossary.Configuration;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Domain.Glossary.IoC
{
    public class GlossaryDomainRegistry : Registry
    {
        public GlossaryDomainRegistry()
        {
            For<IDefinitionRepository>()
                .Use<DefinitionRepository>()
                .SelectConstructor(() => new DefinitionRepository(new Configuration.Configuration()));
            For<ICsvRepository<DefinitionsMap>>().Use<CsvRepostory<DefinitionsMap>>().Singleton();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            
        }
    }
}
