using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Nest;
using StructureMap.Diagnostics.TreeView;

namespace NHS111.Models.Models.Business.PathwaySearch
{
    [ElasticsearchType(IdProperty = "PathwayDigitalId", Name = "pathway")]
    public class PathwaySearchResult
    {
        [String(Name = "PathwayTitle", Index = FieldIndexOption.NotAnalyzed)]
        public string PathwayTitle { get; set; }

        [String(Name = "DigitalDescriptions", Index = FieldIndexOption.Analyzed)]
        public List<string> Title { get; set; }

        [String(Name = "KP_Use", Index = FieldIndexOption.Analyzed)]
        public string Description { get; set; }

        [String(Name = "PW_DID", Index = FieldIndexOption.NotAnalyzed)]
        public string PathwayDigitalId { get; set; }

        [String(Name = "PW_ID", Index = FieldIndexOption.NotAnalyzed)]
        public string PathwayNo { get; set; }

        [String(Name = "PW_Gender", Index = FieldIndexOption.NotAnalyzed)]
        public List<string> Gender { get; set; }

        [String(Name = "PW_Age", Index = FieldIndexOption.NotAnalyzed)]
        public List<string> AgeGroup { get; set; }

        [Text(Ignore = true)]
        public double? Score { get; set; }
    }
}
