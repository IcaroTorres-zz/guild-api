using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;

namespace Infrastructure.Cache
{
    public static class CacheAttributeExtensions
    {
        public static string GenerateCacheKey(this HttpRequest request)
        {
            var cacheKeyBuilder = RemoveUnorderedQueryString(request.Path)
                .Append(BuildOrderedQueryString(request.Query))
                .Append(BuildHeadersKey(request.Headers));

            return cacheKeyBuilder.ToString();
        }

        private static StringBuilder RemoveUnorderedQueryString(PathString path)
        {
            var pathBuilder = new StringBuilder(path);

            var queryStartIndex = path.Value.IndexOf('?');
            if (queryStartIndex > -1) pathBuilder.Remove(path.Value.IndexOf('?'), path.Value.Length - queryStartIndex);

            return pathBuilder;
        }

        private static StringBuilder BuildOrderedQueryString(IQueryCollection queryCollection)
        {
            var queryStringBuilder = new StringBuilder();

            if (queryCollection.Any())
            {
                var isFirstPair = true;

                queryStringBuilder = queryCollection.OrderBy(q => q.Key)
                    .Aggregate(queryStringBuilder, (builder, currentPair) =>
                    {
                        builder.Append(isFirstPair ? "?" : "&")
                            .Append(currentPair.Key)
                            .Append('=')
                            .Append(currentPair.Value);
                        isFirstPair = false;
                        return builder;
                    });
            }

            return queryStringBuilder;
        }

        private static StringBuilder BuildHeadersKey(IHeaderDictionary headers)
        {
            var headersKeyBuilder = headers.Where(header =>
                !string.IsNullOrWhiteSpace(header.Value) &&
                !header.Key.Contains("token", StringComparison.InvariantCultureIgnoreCase) &&
                !header.Key.Contains("referer", StringComparison.InvariantCultureIgnoreCase) &&
                !header.Key.Contains("authorization", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(q => q.Key)
                .Aggregate(new StringBuilder(), (headerBuilder, header) => headerBuilder
                    .Append('|').Append(header.Key).Append('=').Append(header.Value));

            return headersKeyBuilder;
        }
    }
}