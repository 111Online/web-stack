namespace NHS111.Models.Models.Web {
    using System.Collections.Generic;
    using Domain;

    public class SearchJourneyViewModel
        : JourneyViewModel {

        public string SanitisedSearchTerm { get; set; }

        public IEnumerable<SearchResultViewModel> Results { get; set; }
        public bool HasSearched { get { return SanitisedSearchTerm != null; } }
        public IEnumerable<CategoryWithPathways> AllTopics { get; set; }

        public SearchJourneyViewModel() {
            Results = new List<SearchResultViewModel>();
        }
    }
}