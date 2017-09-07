using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web
{
    public class JustToBeSafeViewModel: JourneyViewModel
    {
        public int Part { get; set; }
        public List<QuestionWithAnswers> Questions { get; set; }
        public string QuestionsJson { get; set; }
        public string SelectedQuestionId { get; set; }

        public List<QuestionWithAnswers> OrderedQuestions()
        {
            return Questions.OrderBy(questionWithAnswers => questionWithAnswers.Question.Id).ToList();
        } 

        public bool SelectedNoneApply()
        {
            return string.IsNullOrEmpty(SelectedQuestionId);
        }
    }
}