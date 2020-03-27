using DataAccess.Entities;
using Domain.Models;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateResultAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();
            var valueProperty = executedContext.Result.GetType().GetProperty("Value");
            var value = valueProperty?.GetValue(executedContext.Result);
            var validation = value?.GetType().GetProperty(nameof(DomainModel<Guild>.ValidationResult))?.GetValue(value);
            //var validation = validationMethod?.Invoke(value, null);

            if (validation is ApiValidationResult apiValidation && !apiValidation.IsValid)
            {
                executedContext.Result = apiValidation.AsErrorActionResult();
            }
        }
    }
}
