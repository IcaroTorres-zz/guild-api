using Application.Identity;
using FluentValidation;

namespace Infrastructure.Identity
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
