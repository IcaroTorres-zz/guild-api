using System;
using System.Net;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class NotFoundValidationResult : ErrorValidationResult, IErrorValidationResult, IValidationResult
    {
        public NotFoundValidationResult(string resourcePath) : base(resourcePath)
        {
            Status = HttpStatusCode.NotFound;
            Title = Status.ToString();
            AddValidationErrors(string.Empty, $"Unable to find requested {ResourcePath}.");
        }
        public override IActionResult AsErrorActionResult() => new NotFoundObjectResult(AsSerializableError());
    }
}
