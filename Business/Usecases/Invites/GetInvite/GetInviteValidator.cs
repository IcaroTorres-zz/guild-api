using FluentValidation;

namespace Business.Usecases.Invites.GetInvite
{
    public class GetInviteValidator : AbstractValidator<GetInviteCommand>
    {
        public GetInviteValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}