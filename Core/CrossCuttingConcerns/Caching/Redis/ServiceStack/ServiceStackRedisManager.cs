using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;

namespace Core.CrossCuttingConcerns.Caching.Redis.ServiceStack
{
    public class ServiceStackRedisManager : ICacheManager
    {
        private readonly RedisManagerPool manager;
        readonly IConfiguration _configuration;
        public ServiceStackRedisManager(IConfiguration configuration)
        {
            _configuration = configuration;
            manager = new RedisManagerPool(_configuration["Redis:Host"]);
        }

        public void Add(string key, object value, int duration)
        {
            using (var client = manager.GetClient())
            {
                client.Set(key, value, TimeSpan.FromMinutes(duration));
            }
        }

        public T Get<T>(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.Get<T>(key);
            }
        }

        public object Get(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.GetValue(key);
            }
        }

        public bool IsAdd(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.ContainsKey(key);
            }
        }

        public void Remove(string key)
        {
            using (var client = manager.GetClient())
            {
                client.Remove(key);
            }
        }

        public void RemoveByPattern(string pattern)
        {
            using (var client = manager.GetClient())
            {
                client.RemoveByPattern("*" + pattern + "*");
            }
        }
    }
}
