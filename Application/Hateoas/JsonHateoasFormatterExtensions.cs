using System;
using System.Linq;
using Application.Hateoas.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace Application.Hateoas
{
	internal static class JsonHateoasFormatterExtensions
	{
		private static T GetService<T>(this HttpContext context)
		{
			return (T) context.RequestServices.GetService(typeof(T));
		}
		
		internal static T GetPropertyAsValue<T>(this object source, string propertyName)
		{
			return (T) source.GetType().GetProperty(propertyName)?.GetValue(source);
		}

		internal static ResourceDto AddRequiredLinks(this ResourceDto resource, Type targetResourceType, HttpContext context)
		{
			var urlHelperFactory = context.GetService<IUrlHelperFactory>();
			var actionContextAccessor = context.GetService<IActionContextAccessor>();
			var actionDescriptorCollectionProvider = context.GetService<IActionDescriptorCollectionProvider>();
			var hateoasOptions = context.GetService<IOptions<HateoasOptions>>().Value;
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);

			foreach (var link in hateoasOptions.RequiredLinks.Where(r => r.ResourceType == targetResourceType))
			{
				if (!link.CheckAvailability(resource.Data)) continue;

				if (!(actionDescriptorCollectionProvider.ActionDescriptors.Items.FirstOrDefault(x =>
					x.AttributeRouteInfo.Name == link.Name) is { } route)) continue;

				var routeValues = link.GetRouteValues(resource.Data);
				if (!(urlHelper.Link(link.Name, routeValues)?.ToLower() is { } url)) continue;
					
				var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
				resource.Links.Add(new LinkDto(link.Name, url, method));
			}

			return resource;
		}
	}
}