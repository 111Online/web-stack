using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHS111.Business.Glossary.Api.Models;

namespace NHS111.Domain.Glossary
{
    public interface IDefinitionRepository
    {
        IEnumerable<DefinedTerm> List();
    }
}
