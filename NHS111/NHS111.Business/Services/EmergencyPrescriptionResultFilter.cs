using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Services
{
    public class EmergencyPrescriptionResultFilter : ISearchResultFilter, ICategoryFilter
    {
        private ICCGDetailsService _ccgService;

        public EmergencyPrescriptionResultFilter(ICCGDetailsService ccgService)
        {
            _ccgService = ccgService;
        }

        public async Task<IEnumerable<PathwaySearchResult>> Filter(IEnumerable<PathwaySearchResult> resultsToFilter, IDictionary<string, string> parameters)
        {
            if(!parameters.ContainsKey("postcode")) throw new ArgumentException("Requires postcode key param");

            var ccg = await _ccgService.FillCCGDetailsModel(parameters["postcode"]);
            var results = resultsToFilter;
            if (ccg.PharmacyServicesAvailable == false)
            {
                // If no pharmacy services available then area is not in EP pilot therefore filter out EP pathway
                results = resultsToFilter.Where(result => result.PathwayNo != "PW1827");
            }

            return results;
        }

        public async Task<IEnumerable<CategoryWithPathways>> Filter(IEnumerable<CategoryWithPathways> resultsToFilter, IDictionary<string, string> parameters)
        {
            if(!parameters.ContainsKey("postcode")) throw new ArgumentException("Requires postcode key param");

            var ccg = await _ccgService.FillCCGDetailsModel(parameters["postcode"]);
            var results = resultsToFilter;
            if (ccg.PharmacyServicesAvailable == false)
            {
                // If no pharmacy services available then area is not in EP pilot therefore filter out EP pathway
                // This does no recursion so assumes the EP pathway is in a top-level category of its own, and hides that category
                results = resultsToFilter.Where(result =>
                    result.Pathways.Any(val => val.Pathway.PathwayNo == "PW1827") == false);
            }

            return results;
        }
    }
}
