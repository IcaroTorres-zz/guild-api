using System;
using System.Threading.Tasks;

namespace Abstractions.Service
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<object> GetCacheResponseAsync(string cacheKey);
    }
}
