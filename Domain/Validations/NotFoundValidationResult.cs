using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class NotFoundValidationResult : ValidationResult
    {
        public NotFoundValidationResult(string message) : base()
        {
            errors.Add(new ValidationPair(HttpStatusCode.NotFound, message));
            NotFoundResult = new NotFoundObjectResult(AsSerializableError());
        }
        private NotFoundObjectResult NotFoundResult;
        public override IActionResult AsActionResult() => NotFoundResult;
    }
}
