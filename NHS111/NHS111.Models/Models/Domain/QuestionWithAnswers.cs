using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    public class QuestionWithAnswers
    {
        public Question Question { get; set; }
        public List<Answer> Answers { get; set; }
        public Answer Answered { get; set; }
        public IEnumerable<string> Labels { get; set; }
        public IDictionary<string, string> State { get; set; }
        public OutcomeGroup Group { get; set; }
    }
}