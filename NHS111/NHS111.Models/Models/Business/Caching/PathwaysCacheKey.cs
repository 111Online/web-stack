using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class PathwaysCacheKey : CacheKeyBase<IEnumerable<Pathway>>
    {
        public PathwaysCacheKey(bool grouped, bool startingOnly, string gender, int age) 
            : base(String.Format("PathwayGetAll-Grouped{0}-StartingOnly{1}-{2}-{3}", grouped, startingOnly, gender, age))
        {
        }
    }
}
