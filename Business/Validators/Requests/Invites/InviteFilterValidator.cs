using Business.Commands.Invites;
using FluentValidation;

namespace Business.Validators.Requests.Invites
{
  public class InviteFilterValidator : AbstractValidator<InviteFilterCommand>
  {
    public InviteFilterValidator()
    {
      RuleFor(x => x.Count).GreaterThan(0);
    }
  }
}
