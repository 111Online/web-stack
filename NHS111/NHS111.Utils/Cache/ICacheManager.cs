using System.Threading.Tasks;

namespace NHS111.Utils.Cache
{
    public interface ICacheManager<T, in T1>
    {
        Task Set(T key, T1 value);
        Task<T> Read(T key);
    }
}