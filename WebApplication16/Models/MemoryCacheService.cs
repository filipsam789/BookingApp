using Microsoft.Extensions.Caching.Memory;
using System;
using System.Runtime.Caching;
using MemoryCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace WebApplication16.Models
{

    public class MemoryCacheService : ICache
    {
        public readonly MemoryCache _cache;
        public MemoryCacheService() {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public void Set(string key, object value)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) // Adjust the expiration time as needed
            };

            _cache.Set(key, value, cacheItemPolicy.AbsoluteExpiration);
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }
    }

    public interface ICache
    {
        void Set(string key, object value);
        T Get<T>(string key);
    }
}