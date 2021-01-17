using Application.Identity.Models;
using FluentValidation;
using System.Linq;

namespace Application.Identity.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterModelValidator(IdentityContext identityContext)
        {
            RuleFor(x => x.Name).NotEmpty()
                .Must(name => !identityContext.Users.Any(u => u.Name == name))
                .WithMessage(x => $"Username {x.Name} is already taken");

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
