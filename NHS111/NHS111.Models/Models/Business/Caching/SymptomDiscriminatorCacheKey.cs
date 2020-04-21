using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class SymptomDiscriminatorCacheKey : CacheKeyBase<SymptomDiscriminator>
    {
        public SymptomDiscriminatorCacheKey(string id) 
            :base(string.Format("SymptomDisciminator-{0}", id))
        {}
    }
}
