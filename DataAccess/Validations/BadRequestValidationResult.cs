using System;
using System.Net;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class BadRequestValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public BadRequestValidationResult(string resourceName)
        {
            Status = HttpStatusCode.BadRequest;
            Title = Status.ToString();
            AddValidationError(resourceName, $"Malformed request for entity {resourceName}.");
        }
        public override IActionResult AsErrorActionResult() => new BadRequestObjectResult(AsSerializableError());
    }
}
