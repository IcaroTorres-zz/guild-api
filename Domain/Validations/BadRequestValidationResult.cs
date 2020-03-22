using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class BadRequestValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public BadRequestValidationResult(string resourcePath) : base(resourcePath)
        {
            Status = HttpStatusCode.BadRequest;
            Title = Status.ToString();
            AddValidationErrors(string.Empty, $"Malformed request for entity {ResourcePath}.");
        }
        public override IActionResult AsErrorActionResult() => new BadRequestObjectResult(AsSerializableError());
    }
}
