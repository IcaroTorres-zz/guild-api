using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class ConflictValidationResult : ValidationResult
    {
        public ConflictValidationResult(string message) : base()
        {
            errors.Add(new ValidationPair(HttpStatusCode.Conflict, message));
            ConflictResult = new ConflictObjectResult(AsSerializableError());
        }
        private ConflictObjectResult ConflictResult;
        public override IActionResult AsActionResult() => ConflictResult;
    }
}
