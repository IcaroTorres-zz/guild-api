using Abstractions.Service;
using Application.Cache;
using DataAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class CacheInstallerExtension
    {
        public static IServiceCollection AddRedisResponseCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled)
            {
                return services;
            }
            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            return services;
        }
    }
}
