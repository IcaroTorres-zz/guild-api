using Application.Hateoas.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

      string hateoasOutput = string.Empty;

      if (context.ObjectType.GetGenericTypeDefinition() == typeof(Pagination<>))
      {
        var items = (IEnumerable<object>)context.ObjectType.GetProperty("Items").GetValue(context.Object);
        var count = (long)context.ObjectType.GetProperty("Count")?.GetValue(context.Object);
        var pageSize = (int)context.ObjectType.GetProperty("PageSize").GetValue(context.Object);
        var page = (int)context.ObjectType.GetProperty("Page").GetValue(context.Object);
        var pagination = new Pagination<object>(items, count, pageSize, page);

        hateoasOutput = GeneratePaginatedHateoasOutput(context.HttpContext, context.ObjectType, pagination);
      }
      else hateoasOutput = GenerateHateoasOutput(context.HttpContext, context.ObjectType, context.Object);

      context.HttpContext.Response.ContentType = SupportedMediaTypes.Last();
      return context.HttpContext.Response.WriteAsync(hateoasOutput);
    }

    public ContentResult GetResultWithHateoas(HttpContext context, Type targetResourceType, params object[] value)
    {
      return new ContentResult
      {
        Content = GenerateHateoasOutput(context, targetResourceType, value),
        ContentType = SupportedMediaTypes.Last(),
        StatusCode = (int)HttpStatusCode.OK
      };
    }

    private string GeneratePaginatedHateoasOutput(HttpContext context, Type targetType, Pagination<object> pagination)
    {
      var itemType = targetType.GetGenericArguments().FirstOrDefault();
      var innerResourceValues = pagination.Items.Select(item => WrapDataWithHateoas(itemType, item, context));

      var paginatedResource = new Pagination<ResourceDTO>(innerResourceValues,
          pagination.Count, pagination.PageSize, pagination.Page);

      ResourceDTO dataWithHateoas = WrapDataWithHateoas(targetType, paginatedResource, context);
      return SerializeHateoasData(dataWithHateoas);
    }

    private string GenerateHateoasOutput(HttpContext context, Type targetResourceType, object value)
    {
      ResourceDTO dataWithHateoas = WrapDataWithHateoas(targetResourceType, value, context);
      return SerializeHateoasData(dataWithHateoas);
    }

    private string SerializeHateoasData(ResourceDTO dataWithHateoas)
    {
      return JsonConvert.SerializeObject(dataWithHateoas, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      });
    }

    private SingleResourceDTO WrapDataWithHateoas(Type targetResourceType, object value, HttpContext context)
    {
      var singleResource = new SingleResourceDTO(value);
      singleResource.AddRequiredLinks(targetResourceType, context);
      return singleResource;
    }

    private PaginationResourceDTO<ResourceDTO> WrapDataWithHateoas(Type targetResourceType,
        Pagination<ResourceDTO> value,
        HttpContext context)
    {
      var paginatedResource = new PaginationResourceDTO<ResourceDTO>(value);
      paginatedResource.AddRequiredLinks(targetResourceType, context);
      return paginatedResource;
    }
  }

  internal static class JsonHateoasFormatterExtentions
  {
    public static T GetService<T>(this HttpContext context)
    {
      return (T)context.RequestServices.GetService(typeof(T));
    }
    public static ResourceDTO AddRequiredLinks(this ResourceDTO resource, Type targetResourceType, HttpContext context)
    {
      var urlHelperFactory = context.GetService<IUrlHelperFactory>();
      var contextAccessor = context.GetService<IActionContextAccessor>();
      var descriptorProvider = context.GetService<IActionDescriptorCollectionProvider>();
      var options = context.GetService<IOptions<HateoasOptions>>().Value;
      var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);

      foreach (var link in options.RequiredLinks.Where(r => r.ResourceType == targetResourceType))
      {
        if (descriptorProvider.ActionDescriptors.Items.FirstOrDefault(
            x => x.AttributeRouteInfo.Name == link.Name) is ActionDescriptor route)
        {
          var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
          var routeValues = link.GetRouteValues(resource.Data);
          var routeValuesToFormUrl = routeValues.Count > 0 ? routeValues : null;
          var url = urlHelper.Link(link.Name, routeValuesToFormUrl).ToLower();
          resource.Links.Add(new LinkDTO(link.Name, url, method));
        }
      }
      return resource;
    }
  }
}
