using System;

namespace Domain.Entities
{
    public partial class Invite : EntityModel<Invite>
    {
        public Invite(Guid guildId, Guid memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
        public virtual Invite BeAccepted()
        {
            Status = InviteStatuses.Accepted;
            Guild.AcceptMember(Member.JoinGuild(Guild));
            return this;
        }
        public virtual Invite BeDeclined()
        {
            Status = InviteStatuses.Declined;
            return this;
        }
        public virtual Invite BeCanceled()
        {
            Status = InviteStatuses.Canceled;
            return this;
        }
    }
}
