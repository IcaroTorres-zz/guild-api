using Application.Hateoas.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
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
            var response = context.HttpContext.Response;
            if (context.Object is SerializableError error)
            {
                var errorOutput = JsonConvert.SerializeObject(error);
                response.ContentType = SupportedMediaTypes.First();
                return response.WriteAsync(errorOutput);
            }

            var hateoasOutput = context.ObjectType.ImplementsIEnumerable()
                ? GetJsonStringWithHateoas(context.HttpContext, context.ObjectType, ((IEnumerable<object>)context.Object).ToArray())
                : GetJsonStringWithHateoas(context.HttpContext, context.ObjectType, context.Object);

            response.ContentType = SupportedMediaTypes.Last();
            return response.WriteAsync(hateoasOutput);
        }

        public ContentResult GetResultWithHateoas(HttpContext context, Type targetResourceType, params object[] values)
        {
            var hateoasOutput = GetJsonStringWithHateoas(context, targetResourceType, values);

            return new ContentResult
            {
                Content = hateoasOutput,
                ContentType = SupportedMediaTypes.Last(),
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        private string GetJsonStringWithHateoas(HttpContext context, Type targetResourceType, params object[] values)
        {
            ResourceDTO dataWithHateoas;
            if (targetResourceType.ImplementsIEnumerable())
            {
                if (targetResourceType.GetGenericArguments().FirstOrDefault() is Type itemType)
                {
                    var InnerResourceValues = values.Select(value => WrapDataWithHateoas(itemType, value, context)).ToArray();
                    dataWithHateoas = WrapDataWithHateoas(targetResourceType, InnerResourceValues, context);
                }
                else
                {
                    dataWithHateoas = WrapDataWithHateoas(targetResourceType, values, context);
                }
            }
            else
            {
                dataWithHateoas = WrapDataWithHateoas(targetResourceType, values[0], context);
            }

            return JsonConvert.SerializeObject(dataWithHateoas, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        private SingleResourceDTO WrapDataWithHateoas(Type targetResourceType, object value, HttpContext context)
        {
            return new SingleResourceDTO(value).AddRequiredLinks(targetResourceType, context) as SingleResourceDTO;
        }
        private ArrayResourceDTO<ResourceDTO> WrapDataWithHateoas(Type targetResourceType, ResourceDTO[] value, HttpContext context)
        {
            return new ArrayResourceDTO<ResourceDTO>(value).AddRequiredLinks(targetResourceType, context) as ArrayResourceDTO<ResourceDTO>;
        }
    }

    internal static class JsonHateoasFormatterExtentions
    {
        public static bool ImplementsIEnumerable(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable)) && type.GetGenericArguments().Any();
        }

        public static T GetService<T>(this HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }
        public static ResourceDTO AddRequiredLinks(this ResourceDTO resource, Type targetResourceType, HttpContext context)
        {
            var urlHelperFactory = context.GetService<IUrlHelperFactory>();
            var contextAccessor = context.GetService<IActionContextAccessor>();
            var actionDescriptorProvider = context.GetService<IActionDescriptorCollectionProvider>();
            var options = context.GetService<IOptions<HateoasOptions>>().Value;
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);

            foreach (var linkRequirement in options.RequiredLinks.Where(r => r.ResourceType == targetResourceType))
            {
                var route = actionDescriptorProvider.ActionDescriptors.Items.FirstOrDefault(x => x.AttributeRouteInfo.Name == linkRequirement.Name);
                if (route != null)
                {
                    var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
                    var routeValues = linkRequirement.GetRouteValues(resource.Data);
                    var routeValuesToFormUrl = routeValues.Count > 0 ? routeValues : null;
                    var url = urlHelper.Link(linkRequirement.Name, routeValuesToFormUrl).ToLower();
                    resource.Links.Add(new LinkDTO(linkRequirement.Name, url, method));
                }
            }
            return resource;
        }
    }
}
