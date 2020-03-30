using Domain.Entities;
using Domain.Models;
using Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Text;

namespace Application.ActionFilters.Extensions
{
    public static class ActionFiltersExtensions
    {
        public static ActionExecutedContext EnableResultValidation(this ActionExecutedContext executedContext)
        {
            var value = executedContext.Result.GetPropertyValue<object>(nameof(OkObjectResult.Value));
            var validation = value.GetPropertyValue<IApiValidationResult>(nameof(DomainModel<Guild>.ValidationResult));

            if (validation is IApiValidationResult apiValidation)
            {
                if (apiValidation.IsValid)
                {
                    var entityValue = value.GetPropertyValue(nameof(DomainModel<Guild>.Entity));
                    executedContext.Result.SetPropertyValue(nameof(OkObjectResult.Value), entityValue);
                }
                else
                {
                    executedContext.HttpContext.Response.StatusCode = apiValidation.Status;
                    executedContext.Result = apiValidation.AsErrorActionResult();
                }
            }

            return executedContext;
        }

        public static string GenerateCacheKeyFromRequest(this HttpRequest request)
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

        private static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj?.GetType()?.GetProperty(propertyName)?.GetValue(obj);
        }

        private static TValue GetPropertyValue<TValue>(this object obj, string propertyName)
        {
            return (TValue)obj?.GetPropertyValue(propertyName);
        }

        private static void SetPropertyValue(this object target, string propertyName, object value)
        {
            target?.GetType()?.GetProperty(propertyName)?.SetValue(target, value);
        }
    }
}
