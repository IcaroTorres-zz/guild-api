using DataAccess.Entities;
using DataAccess.Validations;
using Domain.Unities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : ActionFilterAttribute
    {
        public IUnitOfWork UnitOfWork { get; protected set; }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            UnitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>().Begin();
            
            var executedContext = await next();
            var okResult = executedContext.Result as OkObjectResult;
            var createdResult = executedContext.Result as CreatedResult;
            var entityValue =  (okResult?.Value ?? createdResult?.Value) as BaseEntity;

            if (entityValue is BaseEntity && entityValue.Validate() is ErrorValidationResult errorResult)
            {
                executedContext.HttpContext.Response.StatusCode = (int) errorResult.Status;
                executedContext.Result = errorResult.AsErrorActionResult();
            }

            if (executedContext.Exception == null && (executedContext.HttpContext.Response.StatusCode < 400))
            {
                UnitOfWork.Commit();
            }
            else
            {
                UnitOfWork.RollbackTransaction();
            }
        }
    }
}
