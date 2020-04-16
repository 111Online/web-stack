using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class QuestionWithAnswersByPathwayCacheKey :ICacheKey<QuestionWithAnswers>
    {
        private readonly string _cachekey;
        public QuestionWithAnswersByPathwayCacheKey(string pathwayId)
        {
            _cachekey = String.Format("GetFirstQuestion-{0}", pathwayId);
        }

        public string CacheKey
        {
            get { return _cachekey; }
        }
        public bool ValidToAdd(QuestionWithAnswers value)
        {
            return value != null && value.Labels != null && value.Labels.Any() && value.Question != null;
        }
    }
}
