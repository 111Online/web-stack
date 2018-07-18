using System.Linq;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web {
    using System.Collections.Generic;
    using Domain;
    using NHS111.Web;
    using NHS111.Models.Models.Web;

    [Validator(typeof(SearchJourneyViewModelValidator))]
    public class SearchJourneyViewModel : JourneyViewModel
    {
        public string SanitisedSearchTerm { get; set; }
        public IEnumerable<SearchResultViewModel> Results { get; set; }
        public IEnumerable<CategoryWithPathways> Categories { get; set; }
        public bool HasResults { get; set; }

        public SearchJourneyViewModel() {
            Results = new List<SearchResultViewModel>();
        }

        public string OtherProblemsURL(UserInfo model)
        {
            var PWID = "";
            if (model.Demography.Gender == "Male")
            {
                if (model.Demography.Age >= 16)
                    PWID = "PW1346";
                else
                    PWID = "PW1349";
            }

            if (model.Demography.Gender == "Female")
            {
                if (model.Demography.Age >= 16)
                    PWID = "PW1345";
                else
                    PWID = "PW1348";
            }

            return "/" + PWID + "/" + model.Demography.Gender + "/" + model.Demography.Age + "/start";
        }
    }
}