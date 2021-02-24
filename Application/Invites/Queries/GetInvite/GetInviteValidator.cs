using FluentValidation;

namespace Application.Invites.Queries.GetInvite
{
    public class GetInviteValidator : AbstractValidator<GetInviteCommand>
    {
        public GetInviteValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}