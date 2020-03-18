using Abstractions.Service;
using Application.Cache;
using DataAccess.Entities;
using DataAccess.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActionFilters
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
                await ExecuteNextAsync(next);
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if (cachedResponse != null)
            {
                context.Result = new OkObjectResult(cachedResponse);
                return;
            }

            var executedContext = await ExecuteNextAsync(next);
            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, timeToLive: TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private async Task<ActionExecutedContext> ExecuteNextAsync(ActionExecutionDelegate next)
        {
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult || executedContext.Result is CreatedAtRouteResult)
            {
                var executedValue = executedContext.Result.GetType().GetProperty("Value").GetValue(executedContext.Result);
                if (executedValue is BaseEntity entityValue && entityValue.Validate() is ErrorValidationResult errorResult)
                {
                    executedContext.HttpContext.Response.StatusCode = (int) errorResult.Status;
                    executedContext.Result = errorResult.AsErrorActionResult();
                }
            }
            return executedContext;
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cacheKeyBuilder = new StringBuilder(request.Path);

            if (request.Query.Any())
            {
                cacheKeyBuilder = request.Query
                    .OrderBy(q => q.Key)
                    .Aggregate(
                        cacheKeyBuilder,
                        (orderedQueryBuilder, currentQueryStringPair) =>
                        {
                            var previousFullValue = orderedQueryBuilder.Append(orderedQueryBuilder.ToString() == request.Path.ToString() ? "?" : "&").ToString();
                            orderedQueryBuilder.Clear().Append($"{previousFullValue}{currentQueryStringPair.Key}={currentQueryStringPair.Value}");
                            return orderedQueryBuilder;
                        });
            }

            cacheKeyBuilder = request.Headers
                .Where(header => !string.IsNullOrWhiteSpace(header.Value) && !header.Key.ToLowerInvariant().Contains("token"))
                .Aggregate(cacheKeyBuilder, (headerBuilder, header) => headerBuilder.Append($"|{header.Key}={header.Value}"));

            return cacheKeyBuilder.ToString();
        }
    }
}
