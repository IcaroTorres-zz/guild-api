using System;
using System.Threading.Tasks;
using Domain.Unities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

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
				await unitOfWork.RollbackTransactionAsync();
			else await unitOfWork.CommitAsync();
		}
	}
}