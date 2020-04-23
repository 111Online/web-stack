using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business.Caching
{
    public class PathwayMetaDataCacheKey : CacheKeyBase<PathwayMetaData>
    {
        public PathwayMetaDataCacheKey(string id) 
            : base(String.Format("PathwayMetaData-{0}", id))
        {
        }
    }
}
