using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public interface IErrorValidationResult : IValidationResult
    {
        HttpStatusCode Status { get; }
        string Title { get; }
        IReadOnlyDictionary<string, List<string>> Errors { get; }
        object AsSerializableError();
        IActionResult AsErrorActionResult();
        IErrorValidationResult AddValidationError(string key, string errorMessage);
        IErrorValidationResult AddValidationErrors(string key, List<string> newErrorMessages);
    }
}
