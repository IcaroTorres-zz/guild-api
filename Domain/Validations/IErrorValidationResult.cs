using System.Collections.Generic;

namespace Domain.Validations
{
    public interface IErrorValidationResult : IValidationResult
    {
        IReadOnlyDictionary<string, List<string>> Errors { get; }
        IErrorValidationResult AddValidationError(string key, string errorMessage);
        IErrorValidationResult AddValidationErrors(string key, List<string> newErrorMessages);
    }
}
