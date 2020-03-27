using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Domain.Validations
{
    public interface IApiValidationResult
    {
        bool IsValid { get; }
        string Title { get; }
        public IReadOnlyDictionary<string, List<string>> Errors { get; }
        object AsSerializableError();
        IActionResult AsErrorActionResult();
    }
}