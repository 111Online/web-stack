using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Glossary.Api.Services
{
    public interface ITermsService
    {
        Task<IEnumerable<DefinedTerm>> ListDefinedTerms();
        Task<IEnumerable<DefinedTerm>> FindContainedTerms(string text);
    }
}
