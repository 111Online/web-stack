using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Cache
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
        bool ValidToAdd(TItem value);
    }
}
