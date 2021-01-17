using Domain.Repositories;
using FluentValidation;
using System;

namespace Business.Usecases.Invites.InviteMember
{
    public class InviteMemberValidator : AbstractValidator<InviteMemberCommand>
    {
        public InviteMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
        {
            RuleFor(x => x.MemberId).NotEmpty();
            RuleFor(x => x.GuildId).NotEmpty();

            When(x => x.MemberId != Guid.Empty && x.GuildId != Guid.Empty, () =>
            {
                RuleFor(x => x.MemberId)
                    .MustAsync((memberId, ct) => memberRepository.ExistsWithIdAsync(memberId, ct))
                    .WithMessage(x => $"Record not found for invited member with given id {x.MemberId}.");

                RuleFor(x => x.GuildId)
                    .MustAsync((guildId, ct) => guildRepository.ExistsWithIdAsync(guildId, ct))
                    .WithMessage(x => $"Record not found for inviting guild with given id {x.GuildId}.");

                RuleFor(x => x)
                    .MustAsync(async (x, ct) => !await memberRepository.IsGuildMemberAsync(x.MemberId, x.GuildId, ct))
                    .WithMessage("Member is already in target guild.");
            });
        }
    }
}