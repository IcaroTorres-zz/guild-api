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
        IErrorValidationResult AddValidationErrors(string key, params string[] newMessages);
    }
}
