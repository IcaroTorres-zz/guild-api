using DataAccess.Entities;
using System;

namespace Domain.Models
{
    public class MembershipModel : DomainModel<Membership>
    {
        public MembershipModel(Membership entity) : base(entity) { }
        public MembershipModel(GuildModel guild, MemberModel member) : base(new Membership())
        {
            Entity.Guild = guild.Entity;
            Entity.Member = member.Entity;
        }
        public MembershipModel RegisterExit()
        {
            Entity.Exit = DateTime.UtcNow;
            return this;
        }
        public TimeSpan GetDuration() => (Entity.Exit ?? DateTime.UtcNow).Subtract(Entity.Entrance);
    }
}
