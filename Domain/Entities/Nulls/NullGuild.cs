using System;

namespace Domain.Entities
{
    public class NullGuild : Guild
    {
        public NullGuild()
        {
            Id = Guid.Empty;
            Name = string.Empty;
        }

        public override void ChangeName(string newName)
        {
        }

        public override Invite Invite(Member member)
        {
            return new NullInvite();
        }

        public override Member AcceptMember(Member member)
        {
            return member;
        }

        public override Member KickMember(Member member)
        {
            return member;
        }
    }
}