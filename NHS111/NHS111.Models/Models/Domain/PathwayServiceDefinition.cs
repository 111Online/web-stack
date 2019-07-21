using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Domain
{
    public class PathwayServiceDefinition
    {
        public string Id { get; set; } //from neo node id
        public string Version { get; set; } //from config for now until 119 includes pathways version
        public string Description { get; set; } //from digital description
        public DateTime EffectiveDate { get; set; } //from version
        public List<Pathway> Pathways { get; set; }
        public QuestionWithAnswers QuestionsAndSets { get; set; }
    }
}
