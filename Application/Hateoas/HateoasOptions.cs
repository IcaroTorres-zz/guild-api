using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Application.Hateoas
{
    public class HateoasOptions
    {
        private readonly List<IRequiredLink> links = new List<IRequiredLink>();
        public IReadOnlyList<IRequiredLink> RequiredLinks => links.AsReadOnly();

        public HateoasOptions() { }

        public HateoasOptions AddLink<T>(string routeName, Func<T, object> getValues = null) where T : class
        {
            Func<T, RouteValueDictionary> getRouteValuesFunc = r => new RouteValueDictionary();
            if (getValues != null)
            {
                getRouteValuesFunc = r => new RouteValueDictionary(getValues(r));
            }

            links.Add(new ResourceLink<T>(typeof(T), routeName, getRouteValuesFunc));

            return this;
        }
    }
}
