using Application.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response != null)
                await _distributedCache.SetAsync(cacheKey, ToByteArray(response),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeToLive });
        }

        public async Task<object> GetCacheResponseAsync(string cacheKey)
        {
            return ToObject(await _distributedCache.GetAsync(cacheKey));
        }

        private byte[] ToByteArray(object value)
        {
            if (value == null) return null;

            using var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, value);
            return stream.ToArray();
        }

        private object ToObject(byte[] bytes)
        {
            if (bytes == null) return null;

            var stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return new BinaryFormatter().Deserialize(stream);
        }
    }
}