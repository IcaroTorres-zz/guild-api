using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var rootException = actionContext.ModelState.Values
                    .Select(v => new FormatException(string.Join(", ", v.Errors.Select(e => e.ErrorMessage))))
                    .Aggregate((inner, ex) => new FormatException(ex.Message, inner));

                throw rootException;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
