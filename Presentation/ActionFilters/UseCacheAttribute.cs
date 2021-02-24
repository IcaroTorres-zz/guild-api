using Application.Common.Abstractions;
using Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseCacheAttribute : ActionFilterAttribute
    {
        private readonly int _timeToLiveSeconds;

        public UseCacheAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = context.HttpContext.Request.GenerateCacheKey();
            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if (cachedResponse != null)
            {
                context.Result = new OkObjectResult(cachedResponse);
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }
    }
}
