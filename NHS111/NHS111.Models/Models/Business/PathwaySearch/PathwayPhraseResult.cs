using Nest;
using System;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(Name = "pathwayPhrase")]
    public class PathwayPhraseResult
    {
        [Text(Name = "CommonPhrase")]
        public string Description { get; set; }

        [Text(Name = "CommonPhrase.phonetic")]
        public string DescriptionPhonetic { get; set; }

        [Text(Name = "CommonPhrase.shingles")]
        public string DescriptionShingles { get; set; }
    }
}
