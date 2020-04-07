using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Application.Extensions
{
	public static class ControllerExtensions
	{
		public static IActionResult NotFoundFor<T>(this ControllerBase controller, Guid id) => controller.NotFound(
			new
			{
				title = "Resource not found.",
				status = (int) HttpStatusCode.NotFound,
				errors = new {id = new string[] {$"{typeof(T).Name} with key '{id}' not found."}}
			});
	}
}