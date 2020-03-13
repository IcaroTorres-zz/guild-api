using System;
using Domain.Validations;

namespace Domain.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; }
        bool IsValid { get; }
        IValidationResult ValidationResult { get; }
        DateTime CreatedDate { get; }
        DateTime ModifiedDate { get; }
        bool Disabled { get; }
        IValidationResult Validate();
        DateTime RegisterCreation();
        DateTime RegisterModification();
    }
}
