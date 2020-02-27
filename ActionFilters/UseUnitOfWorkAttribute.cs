using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using Unities;

namespace ActionFIlters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseUnitOfWorkAttribute : Attribute, IActionFilter
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            UnitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            UnitOfWork.Begin();
        }

        public void OnActionExecuted(ActionExecutedContext context)
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
