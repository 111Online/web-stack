using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "id", Name = "reserved")]
    public class ReservedSearchResult
    {
        [Keyword(Name = "PathwayTitle")]
        public string PathwayTitle { get; set; }

        [Text(Name = "DigitalDescriptions")]
        public List<string> Title { get; set; }
        
        [Text(Name = "KP_Use")]
        public string Description { get; set; }

        [Keyword(Name = "PW_ID")]
        public string PathwayNo { get; set; }

        [Keyword(Name = "PW_Gender")]
        public List<string> Gender { get; set; }

        [Keyword(Name = "PW_Age")]
        public List<string> AgeGroup { get; set; }

        [Keyword(Name = "Reserved")]
        public List<string> ReservedList { get; set; }
    }
}
