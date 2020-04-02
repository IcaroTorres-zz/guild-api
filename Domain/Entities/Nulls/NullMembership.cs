using System;

namespace Domain.Entities
{
    public class NullMembership : Membership
    {
        public NullMembership()
        {
            Id = Guid.Empty;
            Guild = new NullGuild();
            GuildId = Guid.Empty;
            Member = new NullMember();
            MemberId = Guid.Empty;
        }
        public override Membership RegisterExit()
        {
            return this;
        }
        public override TimeSpan GetDuration()
        {
            return DateTime.UtcNow.Subtract(DateTime.UtcNow);
        }
    }
}
