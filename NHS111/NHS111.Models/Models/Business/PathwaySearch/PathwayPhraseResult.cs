using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(Name = "pathwayPhrase")]
    public class PathwayPhraseResult
    {
        [String(Name = "CommonPhrase", Index = FieldIndexOption.Analyzed)]
        public string Description { get; set; }

        [String(Name = "CommonPhrase.phonetic", Index = FieldIndexOption.Analyzed)]
        public string DescriptionPhonetic { get; set; }

        [String(Name = "CommonPhrase.shingles", Index = FieldIndexOption.Analyzed)]
        public string DescriptionShingles{ get; set; }
    }
}
