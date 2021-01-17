using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Domain.Commands
{
    public interface ICreationCommand : ITransactionalCommand, IRequest<IApiResult>
    {
        IUrlHelper GetUrlHelper();
        string GetRouteName();
        Func<dynamic, object> GetRouteValuesFunc();

        void SetupForCreation(IUrlHelper urlHelper, string routeName, Func<dynamic, object> routeValuesFunc);
    }
}