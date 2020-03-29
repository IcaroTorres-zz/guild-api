using Domain.Entities;

namespace Domain.Models
{
    public class InviteModel : DomainModel<Invite>
    {
        public InviteModel(Invite entity) : base(entity) { }
        public InviteModel(GuildModel guild, MemberModel member) : base(new Invite())
        {
            Entity.Guild = guild.Entity;
            Entity.GuildId = guild.Entity.Id;
            Entity.Member = member.Entity;
            Entity.MemberId = member.Entity.Id;
        }
        public virtual InviteModel BeAccepted()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Accepted;
                var memberModel = new MemberModel(Entity.Member);
                var guildModel = new GuildModel(Entity.Guild);
                guildModel.AcceptMember(memberModel.JoinGuild(this));
            }
            return this;
        }
        public virtual InviteModel BeDeclined()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Declined;
            }
            return this;
        }
        public virtual InviteModel BeCanceled()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Canceled;
            }
            return this;
        }
    }
}
