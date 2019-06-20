using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Services
{
    public interface ICategoryFilter
    {
        Task<IEnumerable<CategoryWithPathways>> Filter(IEnumerable<CategoryWithPathways> resultsToFilter, IDictionary<string, string> parameters);
    }
}
