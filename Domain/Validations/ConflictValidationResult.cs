using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public class ConflictValidationResult : ValidationResult
    {
        public ConflictValidationResult(string message) : base()
        {
            var error = new ValidationPair(HttpStatusCode.Conflict, message);
            ConflictResult = new ConflictObjectResult(error);
            errors.Add(error);
        }
        private ConflictObjectResult ConflictResult;
        public override IActionResult AsActionResult() => ConflictResult;
    }
}
