using Domain.Commands;
using Domain.Hateoas;
using Domain.Messages;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Domain.Responses
{
    public interface IApiResult : IActionResult
    {
        object Data { get; }
        bool Success { get; }
        IEnumerable<DomainMessage> Errors { get; }
        IDictionary<string, Link> Links { get; }
        IApiResult SetResult(object result, HttpStatusCode status = HttpStatusCode.OK);
        IApiResult SetCreated(object result, ICreationCommand creationCommand);
        IApiResult SetExecutionError(HttpStatusCode httpStatusCode = HttpStatusCode.Conflict, params DomainMessage[] errors);
        IApiResult SetValidationError(params ValidationFailure[] validationFailures);
        IApiResult IncludeHateoas(IApiHateoasFactory hateoas);
    }
}
