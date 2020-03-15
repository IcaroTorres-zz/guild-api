using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public interface IValidationResult
    {
        bool IsValid { get; }
    }
}