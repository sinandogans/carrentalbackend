using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Core.CrossCuttingConcerns.Caching.Redis.StackExchange
{
    public class StackExchangeRedisCacheManager : ICacheManager
    {
        readonly ConnectionMultiplexer _redis;
        readonly IDatabase _db;
        readonly IServer _server;
        readonly IConfiguration _configuration;
        public StackExchangeRedisCacheManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _redis = ConnectionMultiplexer.Connect(_configuration["Redis:Host"]);
            _db = _redis.GetDatabase();
            _server = _redis.GetServer(_configuration["Redis:Host"]);
        }


        public void Add(string key, object value, int duration)
        {
            _db.StringSet(key, JsonSerializer.Serialize(value), TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(_db.StringGet(key));
        }

        public object Get(string key)
        {
            var value = _db.StringGet(key);
            return value;
        }

        public bool IsAdd(string key)
        {
            var value = _db.StringGet(key);

            if (!value.IsNull)
                return true;
            return false;
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var keys = _server.Keys();
            foreach (var key in keys)
            {
                if (key.ToString().Contains(pattern))
                {
                    Remove(key.ToString());
                }
            }
        }
    }

}
