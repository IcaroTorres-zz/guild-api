using FluentValidation;

namespace Application.Members.Queries.ListMember
{
    public class ListMemberValidator : AbstractValidator<ListMemberCommand>
    {
        public ListMemberValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
        }
    }
}