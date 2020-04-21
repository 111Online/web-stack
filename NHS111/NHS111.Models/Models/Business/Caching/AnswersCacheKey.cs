using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class AnswersCacheKey : CacheKeyBase<Answer[]>
    {

        public AnswersCacheKey(string questionId)
            : base(String.Format("GetAnswersForQuestion-{0}", questionId))
        {}

        public override bool ValidToAdd(Answer[] value)
        {
            return value != null && value.Any();
        }
    }
}
