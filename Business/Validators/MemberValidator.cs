using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Linq;

namespace Business.Validators
{
    public class MemberValidator : BaseValidator<Member>
    {
        public MemberValidator(IMemberRepository memberRepository,
            IGuildRepository guildRepository,
            IValidator<Membership> membershipValidator)
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x)
                .Must(x => x.Equals(memberRepository.Get(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Member)} with given {nameof(Member.Id)} '{x.Id}' already exists.");

            RuleFor(x => x)
                .Must(x => x.Equals(memberRepository.Query(y => y.Name.Equals(x.Name)).SingleOrDefault()))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"A {nameof(Member)} with given {nameof(Member.Name)} '{x.Name}' already exists.");

            RuleFor(x => x)
                .Must(x => x.Memberships.All(m => m.MemberId.Equals(x.Id)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"Not all {nameof(Membership)}s with '{nameof(Membership.MemberId)}' matching '{x.Id}'.");

            RuleFor(x => x.GuildId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithErrorCode(_notFoundCodeString)
                .WithMessage(_notFoundMessage)
                .Unless(x => x.Guild is null);

            RuleFor(x => x.GuildId)
                .Must(x => guildRepository.Exists(y => y.Id.Equals(x.Value)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"{nameof(Guild)} for {nameof(Member.GuildId)} '{x.GuildId}' in {nameof(Member)} not exists.")
                .When(x => x.GuildId.HasValue);

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId.Value)
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"{nameof(Member.GuildId)} and {nameof(Guild)}.{nameof(Member.Guild.Id)} not matching.")
                .Unless(x => x.Guild is null);

            //RuleFor(x => x.Guild).SetValidator(guildValidator);

            RuleForEach(x => x.Memberships).SetValidator(membershipValidator);

            RuleFor(x => x)
                .Must(x => x.Memberships.Any(ms
                    => ms.MemberId == x.Id
                    && ms.GuildId == x.GuildId
                    && ms.Until == null
                    && !ms.Disabled))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"{nameof(Member)} missing active {nameof(Membership)} for {nameof(Member.GuildId)} '{x.GuildId}'.")
                .When(x => x.Memberships.Any() && x.GuildId.HasValue && !x.GuildId.Equals(Guid.Empty));
        }
    }
}
