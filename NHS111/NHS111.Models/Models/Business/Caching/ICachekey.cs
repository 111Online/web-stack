

namespace NHS111.Models.Models.Business.Caching
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
        bool ValidToAdd(TItem value);
    }
}
