using Nest;
using System.Collections.Generic;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "PathwayDigitalId", Name = "pathway")]
    public class PathwaySearchResult
    {
        public const string HighlightPreTags = "<em class='highlight-term'>";
        public const string HighlightPostTags = "</em>";

        [String(Name = "PathwayTitle", Index = FieldIndexOption.NotAnalyzed)]
        public string PathwayTitle { get; set; }

        [String(Name = "DigitalDescriptions", Index = FieldIndexOption.Analyzed)]
        public List<string> Title { get; set; }

        public List<string> DisplayTitle { get; set; }

        [String(Name = "DigitalDescriptions.phonetic", Index = FieldIndexOption.Analyzed)]
        public List<string> TitlePhonetic { get; set; }

        [String(Name = "DigitalDescriptions.shingles", Index = FieldIndexOption.Analyzed)]
        public List<string> TitleShingles { get; set; }

        [String(Name = "KP_Use", Index = FieldIndexOption.Analyzed)]
        public string Description { get; set; }

        [String(Name = "KP_Use.phonetic", Index = FieldIndexOption.Analyzed)]
        public List<string> DescriptionPhonetic { get; set; }

        [String(Name = "KP_Use.shingles", Index = FieldIndexOption.Analyzed)]
        public List<string> DescriptionShingles { get; set; }

        [String(Name = "PW_ID", Index = FieldIndexOption.NotAnalyzed)]
        public string PathwayNo { get; set; }

        [String(Name = "PW_Gender", Index = FieldIndexOption.NotAnalyzed)]
        public List<string> Gender { get; set; }

        [String(Name = "PW_Age", Index = FieldIndexOption.NotAnalyzed)]
        public List<string> AgeGroup { get; set; }

        [Text(Ignore = true)]
        public double? Score { get; set; }

        public static string StripHighlightMarkup(string highlightedTitle)
        {
            return highlightedTitle.Replace(HighlightPreTags, "").Replace(HighlightPostTags, "");
        }
    }
}
