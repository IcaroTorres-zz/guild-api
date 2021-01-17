using System;

namespace Persistence.Entities
{
    public class Member : Domain.Models.Member
    {
        public override string Name { get; protected set; }
        public override bool IsGuildLeader { get; protected set; }
        public override Guid? GuildId { get; protected set; }
        public override Domain.Models.Guild Guild { get; protected set; }
    }
}