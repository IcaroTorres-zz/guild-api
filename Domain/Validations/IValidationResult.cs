using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        object Data { get; }
        HttpStatusCode Status { get; }
        IReadOnlyList<IValidationPair> Errors { get; }
        IValidationResult AddValidationError(HttpStatusCode statusCode, string message);
        string AsSerializedError();
        IActionResult AsActionResult();
        IActionResult AsActionResult(HttpRequest request);
    }
}
