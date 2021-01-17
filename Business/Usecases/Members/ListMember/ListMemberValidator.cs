using FluentValidation;

namespace Business.Usecases.Members.ListMember
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