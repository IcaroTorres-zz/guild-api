using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using FluentValidation;
using System.Net;

namespace Business.Usecases.Members.LeaveGuild
{
    public class LeaveGuildValidator : AbstractValidator<LeaveGuildCommand>
    {
        public LeaveGuildValidator(IMemberRepository memberRepository)
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
                    .WithErrorCode(nameof(HttpStatusCode.UnprocessableEntity));
            });
        }
    }
}