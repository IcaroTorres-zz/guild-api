using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public class NotFoundValidationResult : ValidationResult
    {
        public NotFoundValidationResult(string message) : base()
        {
            var error = new ValidationPair(HttpStatusCode.NotFound, message);
            notFoundResult = new NotFoundObjectResult(error);
            errors.Add(error);
        }
        private NotFoundObjectResult notFoundResult;
        public override IActionResult AsActionResult() => notFoundResult;
    }
}
