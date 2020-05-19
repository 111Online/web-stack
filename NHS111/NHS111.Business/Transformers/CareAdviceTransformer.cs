using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.Transformers
{
    public class CareAdviceTransformer : ICareAdviceTransformer
    {
        public string AsQuestionWithAnswersList(IEnumerable<CareAdvice> careAdvices)
        {
            var questionWithAnswersList = careAdvices.Select(c => new QuestionWithAnswers()
            {
                Question = new Question
                {
                    CareAdviceId = c.Id.Substring(0, c.Id.IndexOf("-", StringComparison.Ordinal)).Replace("X", "x"),
                    Topic = c.Keyword
                },
                Answers = c.Items != null ? c.Items.Select(i => new Answer { Title = i.Text, Order = i.OrderNo + 1 }).ToList() : new List<Answer>(),
                Labels = new[] { "InterimCareAdvice" }
            });

            return JsonConvert.SerializeObject(questionWithAnswersList);
        }
    }

    public interface ICareAdviceTransformer
    {
        string AsQuestionWithAnswersList(IEnumerable<CareAdvice> careAdvices);
    }
}
