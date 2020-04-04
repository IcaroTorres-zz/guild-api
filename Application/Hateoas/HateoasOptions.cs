using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace Application.Hateoas
{
	public class HateoasOptions
	{
		private readonly List<IRequiredLink> _links = new List<IRequiredLink>();

		public IEnumerable<IRequiredLink> RequiredLinks => _links.AsReadOnly();

		public HateoasOptions AddLink<T>(string routeName, Func<T, object> getValues = null) where T : class
		{
			Func<T, RouteValueDictionary> getRouteValuesFunc = r => new RouteValueDictionary();
			if (getValues != null) getRouteValuesFunc = r => new RouteValueDictionary(getValues(r));

			_links.Add(new ResourceLink<T>(typeof(T), routeName, getRouteValuesFunc));

			return this;
		}
	}
}