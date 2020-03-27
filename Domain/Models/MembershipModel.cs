using Domain.Entities;
using Domain.Validations;
using FluentValidation;
using System;

namespace Domain.Models
{
    public class MembershipModel : DomainModel<Membership>
    {
        public MembershipModel(Membership entity) : base(entity) { }
        public MembershipModel(GuildModel guild, MemberModel member) : base(new Membership())
        {
            Entity.Guild = guild.Entity;
            Entity.GuildId = guild.Entity.Id;
            Entity.Member = member.Entity;
            Entity.MemberId = member.Entity.Id;
        }
        public virtual MembershipModel RegisterExit()
        {
            Entity.Until = DateTime.UtcNow;
            return this;
        }
        public virtual TimeSpan GetDuration()
        {
            return (Entity.Until ?? DateTime.UtcNow).Subtract(Entity.Since);
        }
        public override IApiValidationResult Validate()
        {
            RuleFor(x => x.Since).NotEmpty();

            RuleFor(x => x.MemberId).NotEmpty().NotEqual(Guid.Empty);

            RuleFor(x => x.GuildId).NotEmpty().NotEqual(Guid.Empty);

            RuleFor(x => x.Member.Id).Equal(x => x.MemberId).Unless(x => x.Member == null);

            RuleFor(x => x.Guild.Id).Equal(x => x.GuildId).Unless(x => x.Guild == null);

            return base.Validate();
        }
    }
}
