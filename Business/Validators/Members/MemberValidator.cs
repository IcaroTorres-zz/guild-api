using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Linq;

namespace Business.Validators.PosCommands
{
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator(IMemberRepository memberRepository,
            IGuildRepository guildRepository)
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x)
                .Must(x => x.Memberships.All(m => m.MemberId.Equals(x.Id)))
                .WithErrorCode(CommonValidations.ConflictCodeString)
                .WithMessage(x => $"Not all {nameof(Membership)}s with '{nameof(Membership.MemberId)}' matching '{x.Id}'.");

            RuleFor(x => x.GuildId).NotEmpty().Unless(x => x.Guild is null);

            RuleFor(x => x.GuildId)
                .MustAsync(async (guildId, _) => await guildRepository.ExistsWithIdAsync(guildId.Value))
                .WithMessage(x => CommonValidations.ForRecordNotFound(nameof(Guild), x.GuildId.Value))
                .When(x => x.GuildId.HasValue);

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId.Value)
                .WithMessage(x => $"{nameof(Member.GuildId)} and {nameof(Guild)}.{nameof(Member.Guild.Id)} not matching.")
                .Unless(x => x.Guild is null);

            RuleFor(x => x)
                .Must(x => x.Memberships.Any(ms
                    => ms.MemberId == x.Id
                    && ms.GuildId == x.GuildId
                    && ms.Until == null
                    && !ms.Disabled))
                .WithMessage(x => $"{nameof(Member)} missing active {nameof(Membership)} for {nameof(Member.GuildId)} '{x.GuildId}'.")
                .When(x => x.Memberships.Any() && x.GuildId.HasValue && !x.GuildId.Equals(Guid.Empty));
        }
    }
}
