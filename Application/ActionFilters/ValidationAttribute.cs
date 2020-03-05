using DataAccess.Entities;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var rootException = actionContext.ModelState.Values
                    .Select(v => new FormatException(string.Join(", ", v.Errors.Select(e => e.ErrorMessage))))
                    .Aggregate((inner, ex) => new FormatException(ex.Message, inner));

                throw rootException;
            }

            base.OnActionExecuting(actionContext);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if ((await next()).Result is OkObjectResult okObjectResult
                && okObjectResult.Value is BaseEntity entityResult
                && entityResult.Validate().Errors.Any())
            {
                var rootException = entityResult.ValidationResult.Errors
                    .Select(pair => new FormatException($"{pair.Status}: {pair.Message}"))
                    .Aggregate((inner, ex) => new FormatException(ex.Message, inner));

                throw rootException;
            }
        }
    }
}
