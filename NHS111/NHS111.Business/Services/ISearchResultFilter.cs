using NHS111.Models.Models.Business.PathwaySearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHS111.Business.Services
{
    public interface ISearchResultFilter
    {
        Task<IEnumerable<PathwaySearchResult>> Filter(IEnumerable<PathwaySearchResult> resultsToFilter, IDictionary<string, string> parameters);
    }
}
