using System;
using System.Net;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class ConflictValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public ConflictValidationResult(string resourceName)
        {
            Status = HttpStatusCode.Conflict;
            Title = Status.ToString();
            AddValidationError(resourceName, $"{HttpStatusCode.Conflict} with entity {resourceName}.");
        }
        public override IActionResult AsErrorActionResult() => new ConflictObjectResult(AsSerializableError());
    }
}
