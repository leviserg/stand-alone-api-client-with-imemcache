using Microsoft.Extensions.Caching.Memory;

namespace stand_alone_api_client_with_imemcache.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> dataSource, TimeSpan expirationTime)
        {
            // Check if the item exists in cache and return it
            if (_memoryCache.TryGetValue(key, out T cachedData))
            {
                return cachedData;
            }

            T data;

            try
            {
                // Try to load fresh data
                data = await dataSource();

                // Add to cache with absolute expiration time
                _memoryCache.Set(key, data, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime
                });
            }
            catch
            {
                // In case of an exception, check if we have expired data
                if (_memoryCache.Get(key) is T expiredData)
                {
                    return expiredData;
                }

                // Nothing is available, rethrow the exception
                throw;
            }

            return data;
        }
    }
}
