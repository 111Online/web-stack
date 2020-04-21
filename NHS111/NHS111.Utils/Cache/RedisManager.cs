using StackExchange.Redis;
using System.Threading.Tasks;

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
            if (_redis.IsConnected)
                _database.StringSetAsync(key, value);
        }

        public async Task<string> Read(string key)
        {
            return _redis.IsConnected ? (string)await _database.StringGetAsync(key) : string.Empty;
        }
    }
}