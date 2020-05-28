namespace NHS111.Models.Models.Web
{
    using System.Collections.Generic;

    public class SearchResultViewModel
    {
        public string PathwayTitle { get; set; }
        public List<string> Title { get; set; }
        public List<string> DisplayTitle { get; set; }
        public object TitlePhonetic { get; set; }
        public string Description { get; set; }
        public object DescriptionPhonetic { get; set; }
        public string PathwayNo { get; set; }
        public List<string> Gender { get; set; }
        public List<string> AgeGroup { get; set; }
        public object Score { get; set; }
        public string PathwayTitleWithoutSpaces { get { return PathwayTitle != null ? PathwayTitle.Replace(" ", string.Empty) : string.Empty; } }
    }

    public class GuidedSearchResultViewModel : SearchResultViewModel
    {
        
    }
}