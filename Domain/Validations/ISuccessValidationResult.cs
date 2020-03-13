using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public interface ISuccessValidationResult : IValidationResult
    {
        object Data { get; }
        IActionResult AsActionResult(HttpRequest request);
    }
}
