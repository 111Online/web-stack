using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class QuestionWithAnswersCacheKey :ICacheKey<QuestionWithAnswers>
    {
        private readonly string _cacheKey;
        public QuestionWithAnswersCacheKey(string pathwayId, string nodeId, string answer)
        {
            _cacheKey = string.Format("{0}-{1}-{2}", pathwayId, nodeId, answer);
        }
        public string CacheKey
        {
            get { return _cacheKey; }
        }
    }
}
