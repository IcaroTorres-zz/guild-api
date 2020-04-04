using System;
using Microsoft.AspNetCore.Routing;

namespace Application.Hateoas
{
	public class ResourceLink<T> : IRequiredLink where T : class
	{
		private readonly Func<T, RouteValueDictionary> GetValues;

		public ResourceLink(Type resourceType, string name, Func<T, RouteValueDictionary> values)
		{
			ResourceType = resourceType;
			Name = name;
			GetValues = values;
		}

		public string Name { get; }
		public Type ResourceType { get; }

		public RouteValueDictionary GetRouteValues(object input)
		{
			return GetValues(input as T);
		}
	}
}