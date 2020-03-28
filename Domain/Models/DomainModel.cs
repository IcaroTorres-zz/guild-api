using Domain.Entities;
using Domain.Validations;
using FluentValidation;
using System;
using System.Net;

namespace Domain.Models
{
    public abstract class DomainModel<T> : AbstractValidator<T> where T : EntityModel<T>
    {
        protected DomainModel(T entity)
        {
            Entity = entity;
        }
        public virtual T Entity { get; set; }
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

        public static bool operator !=(DomainModel<T> a, DomainModel<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Entity.Id.GetHashCode();
        }

        public virtual IApiValidationResult ValidationResult => Validate();
        public virtual IApiValidationResult Validate()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            RuleFor(x => x.Disabled)
                .NotEqual(true)
                .WithErrorCode(((int)HttpStatusCode.NotFound).ToString());

            return new ApiValidationResult(Validate(Entity));
        }
        public virtual bool IsValid => Validate().IsValid;
    }
    
    public class Invalidator<T> : AbstractValidator<T> where T : EntityModel<T>
    {
        public virtual T Entity { get; }
        public Invalidator(T entity)
        {
            Entity = entity;

            var resourceName = typeof(T).Name;
            RuleFor(x => x)
                .Must(x => x is null)
                .When(x => x.Id != Guid.Empty)
                .WithErrorCode("404")
                .WithName(resourceName)
                .WithSeverity(Severity.Warning)
                .WithMessage($"{resourceName} not found.");

            RuleFor(x => x)
                .Must(x => x.Id != Guid.Empty)
                .WithErrorCode("409")
                .WithName(resourceName)
                .WithSeverity(Severity.Warning)
                .WithMessage($"{resourceName} is invalid for this request.");
        }
        public virtual IApiValidationResult Validate()
        {
            return new ApiValidationResult(Validate(Entity));
        }
    }
}