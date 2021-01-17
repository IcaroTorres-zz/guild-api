using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using FluentValidation;

namespace Business.Usecases.Members.DemoteMember
{
    public class DemoteMemberValidator : AbstractValidator<DemoteMemberCommand>
    {
        public DemoteMemberValidator(IMemberRepository memberRepository)
        {
            RuleFor(x => x.Id).NotEmpty().DependentRules(() =>
            {
                Member member = Member.Null;

                RuleFor(x => x.Id)
                    .MustAsync(async (id, ct) =>
                    {
                        member = await memberRepository.GetForGuildOperationsAsync(id, ct);
                        return !(member is INullObject);
                    })
                    .WithMessage(x => $"Record not found for member with given id {x.Id}.")
                    .WithName(nameof(Member.Id))
                    .Must(_ => !(member.Guild is INullObject))
                    .WithMessage("Member do not heave a guild to leave from.")
                    .WithName(nameof(Member.Guild))
                    .Must(_ => member.IsGuildLeader)
                    .WithMessage("Only a Guild Master can be demoted.")
                    .WithName(nameof(Member.IsGuildLeader));
            });
        }
    }
}