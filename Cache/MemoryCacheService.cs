using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRatesAPI.Cache
{
    public class MemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? GetFromCache<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void SetCache<T>(string key, T value, TimeSpan expirationTime)
        {
            _cache.Set(key, value, expirationTime);
        }
    }
}
