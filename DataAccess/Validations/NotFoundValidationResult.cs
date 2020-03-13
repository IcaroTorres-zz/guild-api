using System;
using System.Net;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class NotFoundValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public NotFoundValidationResult(string resourceName)
        {
            Status = HttpStatusCode.NotFound;
            Title = Status.ToString();
            AddValidationError(resourceName, $"Unable to find requested {resourceName}.");
        }
        public override IActionResult AsActionResult() => new NotFoundObjectResult(AsSerializableError());
    }
}
