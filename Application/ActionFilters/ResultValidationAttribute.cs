using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResultValidationAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();
            var result = executedContext.Result;
            if (result is OkObjectResult okObjectResult
                && okObjectResult.Value is BaseEntity okEntity
                && !okEntity.IsValid)
            {
                executedContext.Result = okEntity.Validate().AsActionResult();
            }

            if (result is CreatedResult createdResult
                && createdResult.Value is BaseEntity createdEntity
                && !createdEntity.IsValid)
            {
                executedContext.Result = createdEntity.Validate().AsActionResult();
            }
        }
    }
}
