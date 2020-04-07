using Application.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
	public static class CacheExtension
	{
		public static IServiceCollection BootstrapCacheService(this IServiceCollection services, IConfiguration configuration)
		{
			var redisCacheSettings = new RedisCacheSettings();
			configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
			services.AddSingleton(redisCacheSettings);

			if (!redisCacheSettings.Enabled) return services;
			services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();
			return services;
		}
	}
}