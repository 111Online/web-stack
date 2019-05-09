using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business.PathwaySearch;

namespace NHS111.Business.Services
{
    public interface ISearchResultFilter
    {
        Task<IEnumerable<PathwaySearchResult>> Filter(IEnumerable<PathwaySearchResult> resultsToFilter, IDictionary<string, string> parameters);
    }
}
