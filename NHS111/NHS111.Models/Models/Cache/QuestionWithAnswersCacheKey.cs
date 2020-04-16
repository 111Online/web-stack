using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Cache
{
    public class QuestionWithAnswersCacheKey : ICacheKey<QuestionWithAnswers>
    {
        private readonly string _cacheKey;

        public QuestionWithAnswersCacheKey(string nodeId, string nodeLabel, string answer)
        {
            _cacheKey = string.Format("{0}-{1}-{2}", nodeLabel, nodeId, answer);
        }
        public string CacheKey { get { return _cacheKey; }  }
        public bool ValidToAdd(QuestionWithAnswers value)
        {
            return value != null && value.Labels != null && value.Question != null; 
        }
    }
}
