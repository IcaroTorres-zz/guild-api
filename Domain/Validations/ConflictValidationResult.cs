using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class ConflictValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public ConflictValidationResult(string resourcePath) : base(resourcePath)
        {
            Status = HttpStatusCode.Conflict;
            Title = Status.ToString();
            AddValidationErrors(string.Empty, $"{HttpStatusCode.Conflict} with entity {ResourcePath}.");
        }
        public override IActionResult AsErrorActionResult() => new ConflictObjectResult(AsSerializableError());
    }
}
