using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Hateoas.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Hateoas
{
	public class JsonHateoasFormatter : OutputFormatter
	{
		public JsonHateoasFormatter()
		{
			SupportedMediaTypes.Add("application/json");
			SupportedMediaTypes.Add("application/json+hateoas");
		}

		public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
		{
			if (context.Object is SerializableError error)
			{
				var errorOutput = JsonConvert.SerializeObject(error);
				context.HttpContext.Response.ContentType = SupportedMediaTypes.First();
				return context.HttpContext.Response.WriteAsync(errorOutput);
			}

			string hateoasOutput;

			if (context.ObjectType.GetGenericTypeDefinition() == typeof(Pagination<>))
			{
				var items = (IEnumerable<object>) context.ObjectType.GetProperty("Items")?.GetValue(context.Object);
				var count = (long) context.ObjectType.GetProperty("Count")?.GetValue(context.Object);
				var pageSize = (int) context.ObjectType.GetProperty("PageSize")?.GetValue(context.Object);
				var page = (int) context.ObjectType.GetProperty("Page")?.GetValue(context.Object);
				var pagination = new Pagination<object>(items, count, pageSize, page);

				hateoasOutput = GeneratePaginatedHateoasOutput(context.HttpContext, context.ObjectType, pagination);
			}
			else
			{
				hateoasOutput = GenerateHateoasOutput(context.HttpContext, context.ObjectType, context.Object);
			}

			context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
			return context.HttpContext.Response.WriteAsync(hateoasOutput);
		}

		public ContentResult GetResultWithHateoas(HttpContext context, Type targetResourceType, params object[] value)
		{
			return new ContentResult
			{
				Content = GenerateHateoasOutput(context, targetResourceType, value),
				ContentType = SupportedMediaTypes.Last(),
				StatusCode = (int) HttpStatusCode.OK
			};
		}

		private string GeneratePaginatedHateoasOutput(HttpContext context, Type targetType,
			Pagination<object> pagination)
		{
			var itemType = targetType.GetGenericArguments().FirstOrDefault();
			var innerResourceValues = pagination.Items
				.Select(item => WrapDataWithHateoas(itemType, item, context));

			var paginatedResource = new Pagination<ResourceDTO>(
				innerResourceValues, pagination.Count, pagination.PageSize, pagination.Page);

			ResourceDTO dataWithHateoas = WrapDataWithHateoas(targetType, paginatedResource, context);
			return SerializeHateoasData(dataWithHateoas);
		}

		private static string GenerateHateoasOutput(HttpContext context, Type targetResourceType, object value)
		{
			ResourceDTO dataWithHateoas = WrapDataWithHateoas(targetResourceType, value, context);
			return SerializeHateoasData(dataWithHateoas);
		}

		private static string SerializeHateoasData(ResourceDTO dataWithHateoas)
		{
			return JsonConvert.SerializeObject(dataWithHateoas, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

		private static SingleResourceDTO WrapDataWithHateoas(Type targetResourceType, object value, HttpContext context)
		{
			var singleResource = new SingleResourceDTO(value);
			singleResource.AddRequiredLinks(targetResourceType, context);
			return singleResource;
		}

		private static PaginationResourceDto<ResourceDTO> WrapDataWithHateoas(Type targetResourceType,
			Pagination<ResourceDTO> value,
			HttpContext context)
		{
			var paginatedResource = new PaginationResourceDto<ResourceDTO>(value);
			paginatedResource.AddRequiredLinks(targetResourceType, context);
			return paginatedResource;
		}
	}
}