using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class GroupedPathwaysCacheKey : CacheKeyBase<IEnumerable<GroupedPathways>>
    {
        public GroupedPathwaysCacheKey(bool grouped, bool startingOnly) 
            :base(String.Format("PathwaysGetGrouped-Grouped{0}-StartingOnly{1}", grouped, startingOnly))
        { }
    }
}
