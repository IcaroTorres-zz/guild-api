using Microsoft.AspNetCore.Routing;
using System;

namespace Application.Hateoas
{
  public interface IRequiredLink
  {
    Type ResourceType { get; }
    string Name { get; }
    RouteValueDictionary GetRouteValues(object input);
  }
}
