using System;

namespace Domain.Entities
{
    public class NullMember : Member
    {
        public NullMember()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            IsGuildMaster = false;
            GuildId = Guid.Empty;
            Guild = new NullGuild();
        }
        public override void ChangeName(string newName) { }
        public override Member JoinGuild(Guild guild)
        {
            return this;
        }
        public override Member BePromoted()
        {
            return this;
        }
        public override Member BeDemoted()
        {
            return this;
        }
        public override Member LeaveGuild()
        {
            return this;
        }
    }
}
