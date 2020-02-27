using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Api.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                throw new ArgumentException(string.Join(" | ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
