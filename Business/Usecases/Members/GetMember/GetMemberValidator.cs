using FluentValidation;

namespace Business.Usecases.Members.GetMember
{
    public class GetMemberValidator : AbstractValidator<GetMemberCommand>
    {
        public GetMemberValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}