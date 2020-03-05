using System;
using Domain.Validations;

namespace Domain.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; }
        bool IsValid { get; }
        IValidationResult ValidationResult { get; }
        IValidationResult Validate();
        DateTime RegisterCreation();
        DateTime RegisterModification();
    }
}
