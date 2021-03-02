using Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Common.Results
{
    [ExcludeFromCodeCoverage]
    public class FailExecutionResult : OkObjectResult, IApiResult
    {
        public FailExecutionResult(HttpStatusCode status = HttpStatusCode.Conflict, params Error[] errors)
            : base(new Output())
        {
            Data = null;
            Success = false;
            Status = status;
            StatusCode = (int)Status;
            Errors = errors;
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
