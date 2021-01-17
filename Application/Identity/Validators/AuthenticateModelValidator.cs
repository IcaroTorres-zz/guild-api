using Application.Identity.Models;
using FluentValidation;

namespace Application.Identity.Validators
{
    public class AuthenticateModelValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
