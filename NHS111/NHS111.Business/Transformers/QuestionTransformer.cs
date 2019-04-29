using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Transformers
{
    public class QuestionTransformer : IQuestionTransformer
    {
        public IEnumerable<QuestionWithAnswers> AsQuestionWithAnswersList(IEnumerable<QuestionWithAnswers> questions)
        {
            foreach (var questionWithAnswers in questions)
            {
                questionWithAnswers.Answers = TransformAnswers(questionWithAnswers.Answers);
            }
            return questions;
        }

        public QuestionWithAnswers AsQuestionWithAnswers(QuestionWithAnswers question)
        {
            question.Answers = TransformAnswers(question.Answers);
            return question;
        }

        public QuestionWithDeadEnd AsQuestionWithDeadEnd(QuestionWithAnswers question)
        {
            var questionWithDeadEnd = JsonConvert.SerializeObject(question);
            return JsonConvert.DeserializeObject<QuestionWithDeadEnd>(questionWithDeadEnd);
        }

        public QuestionWithPathwaySelection AsQuestionWithPathwaySelection(QuestionWithAnswers question)
        {
            var questionWithPathwaySelection = JsonConvert.SerializeObject(question);
            return JsonConvert.DeserializeObject<QuestionWithPathwaySelection>(questionWithPathwaySelection);
        }

        public IEnumerable<Answer> AsAnswers(IEnumerable<Answer> answers)
        {
            answers = TransformAnswers(answers);
            return answers;
        }

        private static List<Answer> TransformAnswers(IEnumerable<Answer> answers)
        {
            if (answers != null)
                return answers.Select(answer =>
                {
                    answer.Title = answer.Title.FirstToUpper();
                    return answer;
                }).ToList();
            return null;
        }
    }

    public interface IQuestionTransformer
    {
        IEnumerable<QuestionWithAnswers> AsQuestionWithAnswersList(IEnumerable<QuestionWithAnswers> questions);
        QuestionWithAnswers AsQuestionWithAnswers(QuestionWithAnswers question);
        IEnumerable<Answer> AsAnswers(IEnumerable<Answer> answers);
        QuestionWithDeadEnd AsQuestionWithDeadEnd(QuestionWithAnswers question);
        QuestionWithPathwaySelection AsQuestionWithPathwaySelection(QuestionWithAnswers question);
    }
}