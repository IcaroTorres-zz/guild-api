using Application.Common.Abstractions;
using Domain.Common;
using Domain.Models;
using FluentValidation;
using System.Net;

namespace Application.Members.Commands.DemoteMember
{
    public class DemoteMemberValidator : AbstractValidator<DemoteMemberCommand>
    {
        public DemoteMemberValidator(IMemberRepository memberRepository)
        {
            RuleFor(x => x.Id).NotEmpty().DependentRules(() =>
            {
                Member member = Member.Null;

                RuleFor(x => x)
                    .MustAsync(async (x, ct) =>
                    {
                        member = await memberRepository.GetByIdAsync(x.Id, true, ct);
                        return !(member is INullObject);
                    })
                    .WithMessage(x => $"Record not found for member with given id {x.Id}.")
                    .WithName(nameof(Member.Id))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .Must(_ => !(member.Guild is INullObject))
                    .WithMessage("Member do not heave a guild to leave from.")
                    .WithName(nameof(Member.Guild))
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity))

                    .Must(_ => member.IsGuildLeader)
                    .WithMessage("Only a Guild Master can be demoted.")
                    .WithName(nameof(Member.IsGuildLeader))
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}