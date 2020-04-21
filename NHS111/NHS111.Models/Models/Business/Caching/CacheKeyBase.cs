using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business.Caching
{
    public abstract class CacheKeyBase<TItem> : ICacheKey<TItem>
    {
        private readonly string _cacheKey;
        public CacheKeyBase(string cacheKey)
        {
            _cacheKey = cacheKey;
        }
        public string CacheKey
        {
            get { return _cacheKey; } 
        }
        public virtual bool ValidToAdd(TItem value)
        {
            return value != null;
        }
    }
}
