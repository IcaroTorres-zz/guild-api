using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public class BadRequestValidationResult : ValidationResult
    {
        public BadRequestValidationResult(string message) : base()
        {
            var error = new ValidationPair(HttpStatusCode.BadRequest, message);
            badRequestResult = new BadRequestObjectResult(error);
            errors.Add(error);
        }
        private BadRequestObjectResult badRequestResult;
        public override IActionResult AsActionResult() => badRequestResult;
    }
}
