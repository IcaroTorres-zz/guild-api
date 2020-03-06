using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class BadRequestValidationResult : ValidationResult
    {
        public BadRequestValidationResult(string message) : base()
        {
            errors.Add(new ValidationPair(HttpStatusCode.BadRequest, message));
            badRequestResult = new BadRequestObjectResult(AsSerializableError());
        }
        private BadRequestObjectResult badRequestResult;
        public override IActionResult AsActionResult() => badRequestResult;
    }
}
