using System;
using System.Linq;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class QuestionWithAnswersCacheKey : CacheKeyBase<QuestionWithAnswers>
    {


        private QuestionWithAnswersCacheKey(string cacheKey) : base(cacheKey)
        {}

        public static QuestionWithAnswersCacheKey WithPathwayId(string pathwayId)
        {
            return new QuestionWithAnswersCacheKey(String.Format("GetFirstQuestion-{0}", pathwayId));
        }

        public static QuestionWithAnswersCacheKey WithNodeId(string id)
        {
            return new QuestionWithAnswersCacheKey(String.Format("GetQuestionById-{0}", id));
        }
        public QuestionWithAnswersCacheKey(string nodeId, string nodeLabel, string answer) 
            :base(string.Format("{0}-{1}-{2}", nodeLabel, nodeId, answer))
        {}

        public override bool ValidToAdd(QuestionWithAnswers value)
        {
            return value != null && value.Labels != null && value.Labels.Any() && value.Question != null;
        }

    }
}
