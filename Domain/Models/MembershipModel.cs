using Domain.Entities;
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
    }
}
