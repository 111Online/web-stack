
namespace NHS111.Models.Models.Web
{
    using Domain;
    using FluentValidation.Attributes;
    using System.Collections.Generic;
    using Validators;

    [Validator(typeof(SearchJourneyViewModelValidator))]
    public class SearchJourneyViewModel : JourneyViewModel
    {
        public string SanitisedSearchTerm { get; set; }
        public IEnumerable<SearchResultViewModel> Results { get; set; }
        public IEnumerable<CategoryWithPathways> Categories { get; set; }
        public bool HasResults { get; set; }

        public SearchJourneyViewModel()
        {
            Results = new List<SearchResultViewModel>();
        }

        public Pathway GetOtherProblemsPathway(UserInfo model)
        {
            const string OtherProblemsMaleAdultPathwayNumber = "PW1346";
            const string OtherProblemsMaleChildPathwayNumber = "PW1349";
            const string OtherProblemsFemaleAdultPathwayNumber = "PW1345";
            const string OtherProblemsFemaleChildPathwayNumber = "PW1348";

            var pathway = new Pathway();
            if (model.Demography.Gender == "Male")
            {
                pathway.PathwayNo = model.Demography.Age >= 16 ? OtherProblemsMaleAdultPathwayNumber : OtherProblemsMaleChildPathwayNumber;
            }

            if (model.Demography.Gender == "Female")
            {
                pathway.PathwayNo = model.Demography.Age >= 16 ? OtherProblemsFemaleAdultPathwayNumber : OtherProblemsFemaleChildPathwayNumber;
            }

            return pathway;
        }

        public bool IsReservedCovidSearchTerm
        {
            get
            {
                return SanitisedSearchTerm != null && SearchReservedCovidTerms.SearchTerms.Contains(SanitisedSearchTerm.ToLower());
            }
        }


    }

    public class GuidedSearchJourneyViewModel : SearchJourneyViewModel
    {
    }
}