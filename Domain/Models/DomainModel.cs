using Domain.Entities;
using Domain.Validations;
using FluentValidation;
using System;

namespace Domain.Models
{
    public abstract class DomainModel<T> where T : EntityModel<T>
    {
        protected DomainModel(T entity)
        {
            Entity = entity;
        }
        public virtual T Entity { get; set; }
        public virtual IApiValidationResult ValidationResult { get; protected set; }
        public virtual bool IsValid => ValidationResult.IsValid;

        public virtual DomainModel<T> ApplyValidator(IValidator<T> validator)
        {
            ValidationResult = new ApiValidationResult(validator.Validate(Entity));
            return this;
        }
        public override bool Equals(object obj)
        {
            var compareTo = obj as DomainModel<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Entity.Id.Equals(compareTo.Entity.Id);
        }
        public static bool operator ==(DomainModel<T> a, DomainModel<T> b)
        {
            return (a is null && b is null) || !(a is null || b is null) || a.Equals(b);
        }

        public static bool operator !=(DomainModel<T> a, DomainModel<T> b) => !(a == b);

        public override int GetHashCode() => HashCode.Combine(Entity.Id);
    }
    
    //public class Invalidator<T> : AbstractValidator<T> where T : EntityModel<T>
    //{
    //    public virtual T Entity { get; }
    //    public Invalidator(T entity)
    //    {
    //        Entity = entity;

    //        var resourceName = typeof(T).Name;
    //        RuleFor(x => x)
    //            .Must(x => x is null)
    //            .When(x => x.Id != Guid.Empty)
    //            .WithErrorCode("404")
    //            .WithName(resourceName)
    //            .WithSeverity(Severity.Warning)
    //            .WithMessage($"{resourceName} not found.");

    //        RuleFor(x => x)
    //            .Must(x => x.Id != Guid.Empty)
    //            .WithErrorCode("409")
    //            .WithName(resourceName)
    //            .WithSeverity(Severity.Warning)
    //            .WithMessage($"{resourceName} is invalid for this request.");
    //    }
    //    public virtual IApiValidationResult Validate()
    //    {
    //        return new ApiValidationResult(Validate(Entity));
    //    }
    //}
}