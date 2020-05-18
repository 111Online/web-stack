using Nest;
using System.Collections.Generic;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "PathwayDigitalId", Name = "pathway")]
    public class PathwaySearchResult
    {
        public const string HighlightPreTags = "<em class='highlight-term'>";
        public const string HighlightPostTags = "</em>";

        [Keyword(Name = "PathwayTitle")]
        public string PathwayTitle { get; set; }

        [Text(Name = "DigitalDescriptions")]
        public List<string> Title { get; set; }

        public List<string> DisplayTitle { get; set; }

        [Text(Name = "DigitalDescriptions.phonetic")]
        public List<string> TitlePhonetic { get; set; }

        [Text(Name = "DigitalDescriptions.shingles")]
        public List<string> TitleShingles { get; set; }

        [Text(Name = "KP_Use")]
        public string Description { get; set; }

        [Text(Name = "KP_Use.phonetic")]
        public List<string> DescriptionPhonetic { get; set; }

        [Text(Name = "KP_Use.shingles")]
        public List<string> DescriptionShingles { get; set; }

        [Keyword(Name = "PW_ID")]
        public string PathwayNo { get; set; }

        [Keyword(Name = "PW_Gender")]
        public List<string> Gender { get; set; }

        [Keyword(Name = "PW_Age")]
        public List<string> AgeGroup { get; set; }

        [Text(Ignore = true)]
        public double? Score { get; set; }

        public static string StripHighlightMarkup(string highlightedTitle)
        {
            return highlightedTitle.Replace(HighlightPreTags, "").Replace(HighlightPostTags, "");
        }
    }
}
