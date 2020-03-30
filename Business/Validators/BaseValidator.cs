using Domain.Entities;
using FluentValidation;
using System;
using System.Net;

namespace Business.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : EntityModel<T>
    {
        protected readonly string _notFoundMessage = $"{typeof(T).Name} not found.";
        protected readonly string _conflictFoundMessage = $"{typeof(T).Name} is invalid.";
        protected readonly string _goneMessage = $"{typeof(T).Name} was removed or disabled.";

        protected readonly string _notFoundCodeString = ((int)HttpStatusCode.NotFound).ToString();
        protected readonly string _conflictCodeString = ((int)HttpStatusCode.Conflict).ToString();
        protected readonly string _goneCodeString = ((int)HttpStatusCode.Gone).ToString();

        public BaseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage(_notFoundMessage)
                .WithErrorCode(_notFoundCodeString);

            RuleFor(x => x.Disabled)
                .NotEqual(true)
                .WithMessage(_goneCodeString)
                .WithErrorCode(_goneMessage);
        }
    }
}
