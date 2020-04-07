using System;
using Microsoft.AspNetCore.Routing;

namespace Application.Hateoas
{
	public class ResourceLink<T> : IRequiredLink where T : class
	{
		private readonly Func<T, RouteValueDictionary> GetValues;
		private readonly Func<T, bool> PredicateFunction;

		public ResourceLink(Type resourceType, string name, Func<T, RouteValueDictionary> values, Func<T, bool> predicate = null)
		{
			ResourceType = resourceType;
			Name = name;
			GetValues = values;
			PredicateFunction = predicate ?? (t => true);
		}

		public string Name { get; }
		public Type ResourceType { get; }

		public RouteValueDictionary GetRouteValues(object input)
		{
			return GetValues(input as T);
		}
		public bool CheckAvailability(object input)
		{
			return PredicateFunction(input as T);
		}
	}
}