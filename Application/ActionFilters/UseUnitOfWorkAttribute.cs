using Domain.Unities;
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
            if (executedContext.Exception != null || executedContext.HttpContext.Response.StatusCode >= 400)
            {
                unitOfWork.RollbackTransaction();
            }
            else unitOfWork.Commit();
        }
    }
}
