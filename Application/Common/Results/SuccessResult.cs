using Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Application.Common.Results
{
    [ExcludeFromCodeCoverage]
    public class SuccessResult : OkObjectResult, IApiResult
    {
        public SuccessResult(object data, HttpStatusCode status = HttpStatusCode.OK)
            : base(new Output())
        {
            Data = data;
            Success = true;
            Status = status;
            StatusCode = (int)Status;
            Errors = new List<Error>();
            Value = new Output { Data = Data, Success = Success, Errors = Errors };
        }

        public object Data { get; private set; }
        public bool Success { get; }
        internal HttpStatusCode Status { get; }
        public IEnumerable<Error> Errors { get; }
        public HttpStatusCode GetStatus() => Status;

        public IApiResult IncludeHateoas(IApiHateoasFactory hateoas)
        {
            if (Success) ((Output)Value).Links = hateoas.Create(Data);

            return this;
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
