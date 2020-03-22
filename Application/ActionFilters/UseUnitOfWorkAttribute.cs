using Domain.Validations;
using DataAccess.Unities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Domain.Models;
using DataAccess.Entities;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>().Begin();

            var executedContext = await next();
            var valueProperty =  executedContext.Result.GetType().GetProperty("Value");
            var value = valueProperty?.GetValue(executedContext.Result);
            var validationMethod = value?.GetType().GetMethod(nameof(DomainModel<Guild>.Validate));
            var validation = validationMethod?.Invoke(value, null);
            
            if (validation is IValidationResult)
            {
                if (validation is ErrorValidationResult errorResult)
                {
                    executedContext.HttpContext.Response.StatusCode = (int) errorResult.Status;
                    executedContext.Result = errorResult.AsErrorActionResult();
                }
                else
                {
                    var entityValue = value.GetType().GetProperty(nameof(DomainModel<Guild>.Entity))?.GetValue(value);
                    executedContext.Result.GetType().GetProperty("Value")?.SetValue(executedContext.Result, entityValue);
                }
            }

            if (executedContext.Exception != null || executedContext.HttpContext.Response.StatusCode >= 400)
                unitOfWork.RollbackTransaction();
            else
                unitOfWork.Commit();
        }
    }
}
