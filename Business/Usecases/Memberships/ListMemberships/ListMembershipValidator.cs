using FluentValidation;

namespace Business.Usecases.Memberships.ListMemberships
{
    public class ListMembershipValidator : AbstractValidator<ListMembershipCommand>
    {
        public ListMembershipValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
        }
    }
}