using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class AnswersCacheKey : ICacheKey<Answer[]>
    {
        private readonly string _cachekey;
        public AnswersCacheKey(string questionId)
        {
            _cachekey = String.Format("GetAnswersForQuestion-{0}", questionId);
        }

        public string CacheKey
        {
            get { return _cachekey; }
        }
        public bool ValidToAdd(Answer[] value)
        {
            return value != null && value.Any();
        }
    }
}
