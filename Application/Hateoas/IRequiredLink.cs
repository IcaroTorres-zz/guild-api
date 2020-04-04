using System;
using Microsoft.AspNetCore.Routing;

namespace Application.Hateoas
{
	public interface IRequiredLink
	{
		Type ResourceType { get; }
		string Name { get; }
		RouteValueDictionary GetRouteValues(object input);
	}
}