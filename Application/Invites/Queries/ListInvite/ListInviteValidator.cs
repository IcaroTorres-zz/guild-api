using FluentValidation;

namespace Application.Invites.Queries.ListInvite
{
    public class ListInviteValidator : AbstractValidator<ListInviteCommand>
    {
        public ListInviteValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
        }
    }
}