using Domain.Unities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : ActionFilterAttribute
    {
        public IUnitOfWork UnitOfWork { get; protected set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UnitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>().Begin();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null && (context.HttpContext.Response.StatusCode < 400))
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
