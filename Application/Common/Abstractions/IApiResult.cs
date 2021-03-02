using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Application.Common.Abstractions
{
    public interface IApiResult : IActionResult
    {
        object Data { get; }
        bool Success { get; }
        IEnumerable<Error> Errors { get; }
        HttpStatusCode GetStatus();
        IApiResult IncludeHateoas(IApiHateoasFactory hateoas);
        IApiResult SetData(object newData);
    }
}
