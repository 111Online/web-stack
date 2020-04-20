using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class PathwayCacheKey :CacheKeyBase<Pathway>
    {
        public PathwayCacheKey(string id) 
            :base(String.Format("Pathway-{0}", id))
        {}

        public override bool ValidToAdd(Pathway value)
        {
            return value != null && !String.IsNullOrEmpty(value.Id);
        }
    }
}
