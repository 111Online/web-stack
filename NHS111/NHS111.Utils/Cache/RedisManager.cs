using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Caching;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NHS111.Utils.Cache
{
    public class RedisManager : ICacheManager<string, string>
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly TimeSpan _expiry;
        private readonly string _serverName;
        private readonly TelemetryClient _tc = new TelemetryClient();

        public RedisManager(string connString, int expiryInMinutes = 300)
        {
            _redis = ConnectionMultiplexer.Connect(connString);
            _database = _redis.GetDatabase();
            _expiry = TimeSpan.FromMinutes(expiryInMinutes);

            // Parse server hostname and port for later logging
            var endpoint = _redis.GetEndPoints()?.FirstOrDefault() as DnsEndPoint;
            _serverName = endpoint != null ? $"{endpoint.Host}:{endpoint.Port}" : "unknown host";
        }

        /// <summary>
        /// Writes a Key-Value pair into Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task Set(string key, string value)
        {
            if (_redis.IsConnected)
            {
                var success = true;
                var startTime = DateTimeOffset.UtcNow;
                var sw = Stopwatch.StartNew();
                try
                {
                    await _database.StringSetAsync(key, value, _expiry);
                }
                catch (Exception e)
                {
                    success = false;
                    _tc.TrackException(e);
                }
                finally
                {
                    sw.Stop();
                    var telemetry = new DependencyTelemetry()
                    {
                        Target = _serverName,
                        Name = $"SET Key={key}",
                        Type = "Redis",
                        Duration = sw.Elapsed,
                        Timestamp = startTime,
                        Success = success
                    };
                    _tc.TrackDependency(telemetry);
                }
            }

        }

        public async Task<string> Read(string key)
        {
            if (!_redis.IsConnected)
                return string.Empty;

            var success = true;
            string resultCode = "";
            var startTime = DateTimeOffset.UtcNow;
            var sw = Stopwatch.StartNew();
            try
            {
                var value = await _database.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    // 404 = no entry found in the cache for the given key ("cache miss")
                    resultCode = "404";
                    return null;
                }
                else
                {
                    // 200 = entry found in the cache for the given key ("cache hit")
                    resultCode = "200";
                    return value;
                }
            }
            catch (Exception e)
            {
                success = false;
                resultCode = "500";
                _tc.TrackException(e);
                return string.Empty;
            }
            finally
            {
                sw.Stop();
                _tc.TrackDependency(new DependencyTelemetry()
                {
                    Target = _serverName,
                    Name = $"GET Key={key}",
                    Type = "Redis",
                    Duration = sw.Elapsed,
                    Timestamp = startTime,
                    Success = success,
                    ResultCode = resultCode
                });
            }
        }
    }

    public interface ICacheStore
    {
        Task Add<TItem>(TItem item, ICacheKey<TItem> key);

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



        public async Task Add<TItem>(TItem item, ICacheKey<TItem> key)
        {
            if (key.ValidToAdd(item) && _useCache)
                await _cacheManager.Set(key.CacheKey, JsonConvert.SerializeObject(item));
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
                await Add(item, key);
            }

            return item;
        }
    }
}
