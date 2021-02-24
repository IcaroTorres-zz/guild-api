using Application.Common.Abstractions;
using FluentValidation;
using System.Net;

namespace Application.Members.Commands.CreateMember
{
    public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberValidator(IMemberRepository memberRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (name, ct) => !await memberRepository.ExistsWithNameAsync(name, ct))
                .WithMessage(x => $"Record already exist for member with given name {x.Name}.")
                .WithErrorCode(nameof(HttpStatusCode.Conflict));
        }
    }
}