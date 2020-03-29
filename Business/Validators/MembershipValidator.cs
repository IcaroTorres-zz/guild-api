using Domain.Entities;
using FluentValidation;
using System;

namespace Business.Validators
{
    public class MembershipValidator : BaseValidator<Membership>
    {

        public MembershipValidator()
        {
            RuleFor(x => x.Since).NotEmpty();

            RuleFor(x => x.MemberId).NotEmpty().NotEqual(Guid.Empty);

            RuleFor(x => x.GuildId).NotEmpty().NotEqual(Guid.Empty);

            RuleFor(x => x.Member.Id).Equal(x => x.MemberId).Unless(x => x.Member is null);

            RuleFor(x => x.Guild.Id).Equal(x => x.GuildId).Unless(x => x.Guild is null);
        }
    }
}
