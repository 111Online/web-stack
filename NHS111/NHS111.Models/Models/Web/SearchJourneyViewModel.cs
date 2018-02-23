using System.Linq;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web {
    using System.Collections.Generic;
    using Domain;

    [Validator(typeof(SearchJourneyViewModelValidator))]
    public class SearchJourneyViewModel : JourneyViewModel
    {
        public string SanitisedSearchTerm { get; set; }
        public IEnumerable<SearchResultViewModel> Results { get; set; }
        public IEnumerable<CategoryWithPathways> AllTopics { get; set; }
        public bool HasResults { get; set; }

        public SearchJourneyViewModel() {
            Results = new List<SearchResultViewModel>();
        }
    }
}