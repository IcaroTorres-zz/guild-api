using System;

namespace Domain.Entities
{
    [Serializable]
    public class NullInvite : Invite
    {
        public NullInvite()
        {
            Id = Guid.Empty;
            Status = InviteStatuses.Canceled;
            MemberId = Guid.Empty;
            GuildId = Guid.Empty;
            Member = new NullMember();
            Guild = new NullGuild();
        }
        public override Invite BeAccepted()
        {
            return this;
        }
        public override Invite BeDeclined()
        {
            return this;
        }
        public override Invite BeCanceled()
        {
            return this;
        }
    }
}
