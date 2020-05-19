using NHS111.Models.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHS111.Business.Services
{
    public interface ICategoryFilter
    {
        Task<IEnumerable<CategoryWithPathways>> Filter(IEnumerable<CategoryWithPathways> resultsToFilter, IDictionary<string, string> parameters);
    }
}
