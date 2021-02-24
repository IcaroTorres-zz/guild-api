using Application.Common.Abstractions;
using Application.Common.Responses;
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
        public IEnumerable<ApiError> Errors { get; private set; } = new List<ApiError>();
        public IDictionary<string, LinkResponse> Links { get; private set; } = new Dictionary<string, LinkResponse>(StringComparer.InvariantCultureIgnoreCase);
        public HttpStatusCode GetStatus() => Status;

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

        public IApiResult SetExecutionError(HttpStatusCode httpStatusCode = HttpStatusCode.Conflict, params ApiError[] errors)
        {
            Data = null;
            Status = httpStatusCode;
            Errors = errors;
            Success = false;

            return UpdateResultValue();
        }

        public IApiResult SetValidationError(params ValidationFailure[] validationFailures)
        {
            Data = null;
            Success = false;
            Errors = ExtractAsDomainMessages(validationFailures);
            Status = Enum.TryParse<HttpStatusCode>(validationFailures.FirstOrDefault()?.ErrorCode, true, out var statusFromValidation)
                ? statusFromValidation
                : HttpStatusCode.BadRequest;

            return UpdateResultValue();
        }

        private IApiResult UpdateResultValue()
        {
            Value = new Result { Data = Data, Success = Success, Errors = Errors, Links = Links };
            StatusCode = (int)Status;

            return this;
        }

        private IEnumerable<ApiError> ExtractAsDomainMessages(ValidationFailure[] validationFailures)
        {
            return (validationFailures ?? new ValidationFailure[] { }).Select(x => new ApiError(x.PropertyName, x.ErrorMessage));
        }

        public IApiResult IncludeHateoas(IApiHateoasFactory hateoas)
        {
            if (!Success) return this;

            Links = hateoas.Create(Data);
            return UpdateResultValue();
        }
    }
}
