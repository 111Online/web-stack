using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Transformers
{
    public class QuestionTransformer : IQuestionTransformer
    {
        public string AsQuestionWithAnswersList(string s)
        {
            var questionWithAnswersList = JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(s);
            foreach (var questionWithAnswers in questionWithAnswersList)
            {
                questionWithAnswers.Answers = TransformAnswers(questionWithAnswers.Answers);
            }

            return JsonConvert.SerializeObject(questionWithAnswersList);
        }

        public string AsQuestionWithAnswers(string s)
        {
            var questionWithAnswers = JsonConvert.DeserializeObject<QuestionWithAnswers>(s);
            questionWithAnswers.Answers = TransformAnswers(questionWithAnswers.Answers);
            return JsonConvert.SerializeObject(questionWithAnswers);
        }

        public string AsQuestionWithDeadEnd(string s)
        {
            var questionWithDeadEnd = JsonConvert.DeserializeObject<QuestionWithDeadEnd>(s);
            return JsonConvert.SerializeObject(questionWithDeadEnd);
        }

        public string AsAnswers(string s)
        {
            var answers = JsonConvert.DeserializeObject<List<Answer>>(s);
            return JsonConvert.SerializeObject(TransformAnswers(answers));
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
        string AsQuestionWithAnswersList(string s);
        string AsQuestionWithAnswers(string s);
        string AsAnswers(string s);
        string AsQuestionWithDeadEnd(string s);
    }
}