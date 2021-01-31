using Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Business.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class CreationCommand<TIn, TOut> : MappedResultCommand<TIn, TOut>, ICreationCommand
    {
        private IUrlHelper _urlHelper;
        private string _routeName;
        private Func<dynamic, object> _routeValuesFunc;

        public IUrlHelper GetUrlHelper()
        {
            return _urlHelper;
        }

        public string GetRouteName()
        {
            return _routeName;
        }

        public Func<dynamic, object> GetRouteValuesFunc()
        {
            return _routeValuesFunc;
        }

        public virtual void SetupForCreation(IUrlHelper urlHelper, string routeName, Func<dynamic, object> routeValuesFunc)
        {
            _urlHelper = urlHelper;
            _routeName = routeName;
            _routeValuesFunc = routeValuesFunc;
        }
    }
}