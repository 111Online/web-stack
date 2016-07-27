using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.Glossary.Api.Models;
using NHS111.Domain.Glossary.Configuration;

namespace NHS111.Domain.Glossary
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private ICsvRepository<DefinitionsMap> _csvRepository;

        public DefinitionRepository(ICsvRepository<DefinitionsMap> csvRepository)
        {
            _csvRepository = csvRepository;
        }

        public DefinitionRepository(IConfiguration configuration)
        {
            _csvRepository = new CsvRepostory<DefinitionsMap>(new FileAdapter(configuration.TermsCsvFilePath()));
        }

        public IEnumerable<DefinedTerm> List()
        
        {
            return _csvRepository.List<DefinedTerm>().ToList();
        }

    }
}
