using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Caching;
using StackExchange.Redis;

namespace NHS111.Utils.Cache
{
    public class RedisManager : ICacheManager<string, string>
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisManager(string connString)
        {
            _redis = ConnectionMultiplexer.Connect(connString);
            _database = _redis.GetDatabase();
        }

        public void Set(string key, string value)
        {
            if(_redis.IsConnected)
                _database.StringSetAsync(key, value);
        }

        public async Task<string> Read(string key)
        {
            return _redis.IsConnected ? (string) await _database.StringGetAsync(key) : string.Empty;
        }
    }

    public interface ICacheStore
    {
        void Add<TItem>(TItem item, ICacheKey<TItem> key);

        Task<TItem> Get<TItem>(ICacheKey<TItem> key) where TItem : class;
        Task<TItem> GetOrAdd<TItem>(ICacheKey<TItem> key, Func<Task<TItem>> expression) where TItem : class;
    }

    public class RedisCacheStore : ICacheStore
    {
        private readonly ICacheManager<string, string> _cacheManager;

        private bool _useCache = false;

        public RedisCacheStore(ICacheManager<string, string> cacheManager) 
        {
            _cacheManager = cacheManager;
        #if !DEBUG 
            _useCache=true;
        #endif
        }

        public RedisCacheStore(ICacheManager<string, string> cacheManager, bool useCache)
        {
            _cacheManager = cacheManager;
            _useCache = useCache;
        }



        public void Add<TItem>(TItem item, ICacheKey<TItem> key)
        {
            if(key.ValidToAdd(item) && _useCache)
                _cacheManager.Set(key.CacheKey, JsonConvert.SerializeObject(item));
        }

        public async Task<TItem> Get<TItem>(ICacheKey<TItem> key) where TItem : class
        {
            if (_useCache)
            {
                var cacheVal = await _cacheManager.Read(key.CacheKey);
                if (!String.IsNullOrEmpty(cacheVal))
                    return JsonConvert.DeserializeObject<TItem>(cacheVal);
            }

            return null;

        }

        public async Task<TItem> GetOrAdd<TItem>(ICacheKey<TItem> key, Func<Task<TItem>> expression) where TItem : class
        {
            var item = await Get(key);
            if (item == null)
            {
                item = await expression.Invoke();
                Add(item, key);
            }

            return item;
        }
    }
}
