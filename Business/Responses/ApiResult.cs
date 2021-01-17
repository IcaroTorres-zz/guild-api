using Domain.Commands;
using Domain.Hateoas;
using Domain.Messages;
using Domain.Responses;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace Business.Responses
{
    [ExcludeFromCodeCoverage]
    public class ApiResult : OkObjectResult, IApiResult
    {
        public ApiResult() : base(new Result())
        {
            Status = HttpStatusCode.OK;
            StatusCode = (int)Status;
        }

        public object Data { get; private set; }
        public bool Success { get; private set; } = true;
        internal HttpStatusCode Status { get; private set; } = HttpStatusCode.OK;
        public IEnumerable<DomainMessage> Errors { get; private set; } = new List<DomainMessage>();
        public IDictionary<string, Link> Links { get; private set; } = new Dictionary<string, Link>(StringComparer.InvariantCultureIgnoreCase);

        public IApiResult SetResult(object result, HttpStatusCode status = HttpStatusCode.OK)
        {
            Data = result;
            Status = status;

            return UpdateResultValue();
        }

        public IApiResult SetCreated(object result, ICreationCommand creationCommand)
        {
            if (!Success) return this;

            return new ApiCreatedResult().SetCreated(result, creationCommand);
        }

        public IApiResult SetExecutionError(HttpStatusCode? httpStatusCode = null, params DomainMessage[] errors)
        {
            Data = null;
            Status = httpStatusCode ?? HttpStatusCode.Conflict;
            Errors = errors;
            Success = false;

            return UpdateResultValue();
        }

        public IApiResult SetValidationError(params ValidationFailure[] validationFailures)
        {
            Data = null;
            Errors = ExtractAsDomainMessages(validationFailures);
            Status = HttpStatusCode.BadRequest;
            Success = false;

            return UpdateResultValue();
        }

        private IApiResult UpdateResultValue()
        {
            Value = new Result { Data = Data, Success = Success, Errors = Errors, Links = Links };
            StatusCode = (int)Status;

            return this;
        }

        private IEnumerable<DomainMessage> ExtractAsDomainMessages(ValidationFailure[] validationFailures)
        {
            return (validationFailures ?? new ValidationFailure[] { }).Select(x => new DomainMessage(x.PropertyName, x.ErrorMessage));
        }

        public IApiResult IncludeHateoas(IApiHateoasFactory hateoas)
        {
            if (!Success) return this;

            Links = hateoas.Create(Data);
            return UpdateResultValue();
        }
    }
}
