using System;
using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    public class ServiceDefinition
    {
        public string Id { get; set; }
        public string Version { get; set; } //from config for now until 119 includes pathways version
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public IEnumerable<Pathway> Pathways { get; set; }
        public IEnumerable<QuestionWithAnswers> QuestionsAndSets { get; set; }
    }
}
