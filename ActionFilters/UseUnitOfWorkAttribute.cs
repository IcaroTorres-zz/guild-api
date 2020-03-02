using Abstractions.Unities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : ActionFilterAttribute
    {
        public IUnitOfWork UnitOfWork { get; protected set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UnitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            UnitOfWork.Begin();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            UnitOfWork ??= context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            if (context.Exception == null)
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
