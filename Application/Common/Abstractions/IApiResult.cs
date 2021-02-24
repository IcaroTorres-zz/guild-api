using Application.Common.Responses;
using Application.Common.Results;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Application.Common.Abstractions
{
    public interface IApiResult : IActionResult
    {
        object Data { get; }
        bool Success { get; }
        IEnumerable<ApiError> Errors { get; }
        IDictionary<string, LinkResponse> Links { get; }
        HttpStatusCode GetStatus();
        IApiResult SetResult(object result, HttpStatusCode status = HttpStatusCode.OK);
        IApiResult SetCreated(object result, ICreationCommand creationCommand);
        IApiResult SetExecutionError(HttpStatusCode httpStatusCode = HttpStatusCode.Conflict, params ApiError[] errors);
        IApiResult SetValidationError(params ValidationFailure[] validationFailures);
        IApiResult IncludeHateoas(IApiHateoasFactory hateoas);
    }
}
