using Domain.Entities;
using Domain.Models;
using Domain.Unities;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>().Begin();

            var executedContext = await next();
            var valueProperty = executedContext.Result.GetType().GetProperty("Value");
            var value = valueProperty?.GetValue(executedContext.Result);
            var validation = value?.GetType().GetProperty(nameof(DomainModel<Guild>.ValidationResult))?.GetValue(value);

            if (validation is IApiValidationResult apiValidation)
            {
                if (apiValidation.IsValid)
                {
                    var entityValue = value?.GetType().GetProperty(nameof(DomainModel<Guild>.Entity))?.GetValue(value);
                    executedContext.Result.GetType().GetProperty("Value")?.SetValue(executedContext.Result, entityValue);
                }
                else
                {
                    executedContext.HttpContext.Response.StatusCode = apiValidation.Status;
                    executedContext.Result = apiValidation.AsErrorActionResult();
                }
            }

            if (executedContext.Exception != null || executedContext.HttpContext.Response.StatusCode >= 400)
                unitOfWork.RollbackTransaction();
            else
                unitOfWork.Commit();
        }
    }
}
