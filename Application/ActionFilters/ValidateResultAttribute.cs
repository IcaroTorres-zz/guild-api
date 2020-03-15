using DataAccess.Entities;
using DataAccess.Validations;
using Microsoft.AspNetCore.Mvc;
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
            var okResult = executedContext.Result as OkObjectResult;
            var createdResult = executedContext.Result as CreatedResult;
            var entityValue =  (okResult?.Value ?? createdResult?.Value) as BaseEntity;
            if (entityValue is BaseEntity && entityValue.Validate() is ErrorValidationResult errorInsteadOkResult)
            {
                executedContext.Result = errorInsteadOkResult.AsErrorActionResult();
            }
        }
    }
}
