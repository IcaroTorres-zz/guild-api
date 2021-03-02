using Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Common.Results
{
    [ExcludeFromCodeCoverage]
    public class SuccessCreatedResult : CreatedResult, IApiResult
    {
        public SuccessCreatedResult(object data, ICreationCommand creationCommand)
            : base(GenerateLocation(creationCommand, data), new Output { Data = data, Success = true })
        {
            Data = data;
            Success = true;
            Errors = new List<Error>();
        }

        public object Data { get; private set; }
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }
        public HttpStatusCode GetStatus() => HttpStatusCode.Created;

        public IApiResult IncludeHateoas(IApiHateoasFactory hateoas)
        {
            if (Success) ((Output)Value).Links = hateoas.Create(Data);

            return this;
        }

        private static string GenerateLocation(ICreationCommand creationCommand, object data)
        {
            var urlHelper = creationCommand.GetUrlHelper();
            var routeName = creationCommand.GetRouteName();
            var routeValuesFunction = creationCommand.GetRouteValuesFunc();

            return urlHelper.Link(routeName, routeValuesFunction(data));
        }

        public IApiResult SetData(object newData)
        {
            if (Success)
            {
                Data = newData;
                ((Output)Value).Data = newData;
            }

            return this;
        }
    }
}
