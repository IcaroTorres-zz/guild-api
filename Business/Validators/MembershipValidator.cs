using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using System;

namespace Business.Validators
{
    public class MembershipValidator : BaseValidator<Membership>
    {

        public MembershipValidator(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            RuleFor(x => x.Since).NotEmpty()
            .WithErrorCode(_conflictCodeString)
            .WithMessage($"{nameof(Membership)} must have a {nameof(Membership.Since)} start date.");

            RuleFor(x => x.MemberId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .Must(x => memberRepository.Exists(y => y.Id.Equals(x)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"{nameof(Member)} for {nameof(Membership.MemberId)} '{x.MemberId}' not exists.");

            RuleFor(x => x.GuildId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .Must(x => guildRepository.Exists(y => y.Id.Equals(x)))
                .WithErrorCode(_conflictCodeString)
                .WithMessage(x => $"{nameof(Guild)} for {nameof(Membership.GuildId)} '{x.GuildId}' not exists.");

            RuleFor(x => x.Member.Id).Equal(x => x.MemberId).Unless(x => x.Member is null);

            RuleFor(x => x.Guild.Id).Equal(x => x.GuildId).Unless(x => x.Guild is null);
        }
    }
}
