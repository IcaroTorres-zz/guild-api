using System;
using System.Collections.Generic;
using System.Linq;
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
        var items = context.Object.GetPropertyAsValue<IEnumerable<object>>(nameof(Pagination<object>.Data));
        var count = context.Object.GetPropertyAsValue<long>(nameof(Pagination<object>.Count));
        var pageSize = context.Object.GetPropertyAsValue<int>(nameof(Pagination<object>.PageSize));
        var page = context.Object.GetPropertyAsValue<int>(nameof(Pagination<object>.Page));
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

    private static string GeneratePaginatedHateoasOutput(HttpContext context, Type targetType,
      Pagination<object> pagination)
    {
      var itemType = targetType.GetGenericArguments().FirstOrDefault();
      var innerResourceValues = pagination.Data
        .Select(item => WrapDataWithHateoas(itemType, item, context));

      var paginatedResource = new Pagination<ResourceDto>(
        innerResourceValues, pagination.Count, pagination.PageSize, pagination.Page);

      ResourceDto dataWithHateoas = WrapDataWithHateoas(targetType, paginatedResource, context);
      return SerializeHateoasData(dataWithHateoas);
    }

    private static string GenerateHateoasOutput(HttpContext context, Type targetResourceType, object value)
    {
      ResourceDto dataWithHateoas = WrapDataWithHateoas(targetResourceType, value, context);
      return SerializeHateoasData(dataWithHateoas);
    }

    private static string SerializeHateoasData(ResourceDto dataWithHateoas)
    {
      return JsonConvert.SerializeObject(dataWithHateoas, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      });
    }

    private static SingleResourceDto WrapDataWithHateoas(Type targetResourceType, object value, HttpContext context)
    {
      var singleResource = new SingleResourceDto(value);
      singleResource.AddRequiredLinks(targetResourceType, context);
      return singleResource;
    }

    private static PaginationResourceDto<ResourceDto> WrapDataWithHateoas(Type targetResourceType,
      Pagination<ResourceDto> value,
      HttpContext context)
    {
      var paginatedResource = new PaginationResourceDto<ResourceDto>(value);
      paginatedResource.AddRequiredLinks(targetResourceType, context);
      return paginatedResource;
    }
  }
}