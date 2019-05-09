using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business.PathwaySearch;

namespace NHS111.Business.Services
{
    public class EmergencyPrescriptionResultFilter : ISearchResultFilter
    {
        private ICCGService _ccgService;
        public EmergencyPrescriptionResultFilter(ICCGService ccgService)
        {
            _ccgService = ccgService;
        }
        public Task<IEnumerable<PathwaySearchResult>> Filter(IEnumerable<PathwaySearchResult> resultsToFilter, IDictionary<string, string> parameters)
        {
            if(!parameters.ContainsKey("postcode")) throw new ArgumentException("Requires postcode key param");
            //Implementation Here
           // _ccgService.FillCCGModel()
            throw new NotImplementedException();
        }
    }
}
