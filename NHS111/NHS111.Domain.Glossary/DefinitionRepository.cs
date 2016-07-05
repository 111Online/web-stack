using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Glossary
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private ICsvRepository _csvRepository;

        public DefinitionRepository(ICsvRepository csvRepository)
        {
            _csvRepository = csvRepository;
        }

    }
}
