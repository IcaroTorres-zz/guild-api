using Application.Common.Abstractions;
using Domain.Common;
using Domain.Models;
using FluentValidation;
using System.Net;

namespace Application.Members.Commands.PromoteMember
{
    public class PromoteMemberValidator : AbstractValidator<PromoteMemberCommand>
    {
        public PromoteMemberValidator(IMemberRepository memberRepository)
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
                    .WithName("Id")
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .Must(_ => !(member.GetGuild() is INullObject))
                    .WithMessage("Member do not heave a guild to leave from.")
                    .WithName("Guild")
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity))

                    .Must(_ => !member.IsGuildLeader)
                    .WithMessage("A Guild Master cannot be promoted.")
                    .WithName("IsGuildLeader")
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}