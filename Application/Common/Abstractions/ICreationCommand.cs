using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Common.Abstractions
{
    public interface ICreationCommand : ITransactionalCommand, IRequest<IApiResult>
    {
        IUrlHelper GetUrlHelper();
        string GetRouteName();
        Func<dynamic, object> GetRouteValuesFunc();

        void SetupForCreation(IUrlHelper urlHelper, string routeName, Func<dynamic, object> routeValuesFunc);
    }
}