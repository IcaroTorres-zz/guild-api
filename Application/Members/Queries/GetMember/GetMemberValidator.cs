using FluentValidation;

namespace Application.Members.Queries.GetMember
{
    public class GetMemberValidator : AbstractValidator<GetMemberCommand>
    {
        public GetMemberValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}