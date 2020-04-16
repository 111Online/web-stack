using System.Linq;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class QuestionWithAnswersCacheKey : ICacheKey<QuestionWithAnswers>
    {
        private readonly string _cacheKey;

        public QuestionWithAnswersCacheKey(string id)
        {
            _cacheKey = string.Format("GetQuestionById-{0}", id);
        }

        public QuestionWithAnswersCacheKey(string nodeId, string nodeLabel, string answer)
        {
            _cacheKey = string.Format("{0}-{1}-{2}", nodeLabel, nodeId, answer);
        }
        public string CacheKey { get { return _cacheKey; }  }
        public bool ValidToAdd(QuestionWithAnswers value)
        {
            return value != null && value.Labels !=null && value.Labels.Any() && value.Question != null; 
        }
    }
}
