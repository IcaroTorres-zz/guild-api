using Application.Common.Abstractions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace Application.Common.Results
{
    [ExcludeFromCodeCoverage]
    public class FailValidationResult : OkObjectResult, IApiResult
    {
        public FailValidationResult(params ValidationFailure[] validationFailures)
            : base(new Output())
        {
            Data = null;
            Success = false;
            Errors = validationFailures.ToApiError();
            Status = Enum.TryParse<HttpStatusCode>(validationFailures.FirstOrDefault()?.ErrorCode, true, out var statusFromValidation)
                ? statusFromValidation
                : HttpStatusCode.BadRequest;
            StatusCode = (int)Status;
            Value = new Output { Data = Data, Success = Success, Errors = Errors };
        }

        public object Data { get; }
        public bool Success { get; }
        internal HttpStatusCode Status { get; }
        public IEnumerable<Error> Errors { get; }
        public HttpStatusCode GetStatus() => Status;

        public IApiResult IncludeHateoas(IApiHateoasFactory hateoas)
        {
            return this;
        }

        public IApiResult SetData(object newData)
        {
            return this;
        }
    }
}
