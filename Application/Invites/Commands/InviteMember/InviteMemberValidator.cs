using Application.Common.Abstractions;
using FluentValidation;
using System;
using System.Net;

namespace Application.Invites.Commands.InviteMember
{
    public class InviteMemberValidator : AbstractValidator<InviteMemberCommand>
    {
        public InviteMemberValidator(IMemberRepository memberRepository, IGuildRepository guildRepository)
        {
            RuleFor(x => x.MemberId).NotEmpty();
            RuleFor(x => x.GuildId).NotEmpty();

            When(x => x.MemberId != Guid.Empty && x.GuildId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                    .MustAsync((x, ct) => memberRepository.ExistsWithIdAsync(x.MemberId, ct))
                    .WithMessage(x => $"Record not found for invited member with given id {x.MemberId}.")
                    .WithName(x => nameof(x.MemberId))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .MustAsync((x, ct) => guildRepository.ExistsWithIdAsync(x.GuildId, ct))
                    .WithMessage(x => $"Record not found for inviting guild with given id {x.GuildId}.")
                    .WithName(x => nameof(x.GuildId))
                    .WithErrorCode(nameof(HttpStatusCode.NotFound))

                    .MustAsync(async (x, ct) => !await memberRepository.IsGuildMemberAsync(x.MemberId, x.GuildId, ct))
                    .WithMessage("Member is already in target guild.")
                    .WithName(x => nameof(x.MemberId))
                    .WithErrorCode(nameof(HttpStatusCode.Conflict));
            });
        }
    }
}