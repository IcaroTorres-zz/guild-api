using Abstractions.Entities;
using System;

namespace Implementations.Entities
{
    [Serializable]
    public class Membership : IMembership
    {
        protected Membership() { }

        public Membership(Guild guild, Member member)
        {
            Id =  Guid.NewGuid();
            Guild = guild;
            Member = member;
        }
        public Guid Id { get; protected set; }
        public DateTime Entrance { get; protected set; } = DateTime.UtcNow;
        public DateTime? Exit { get; protected set; }
        public bool Disabled { get; set; } = false;

        public virtual Member Member { get; protected set; }
        public Guid MemberId { get; protected set; }

        public virtual Guild Guild { get; protected set; }
        public Guid GuildId { get; protected set; }

        public IMembership RegisterExit()
        {
            Exit = DateTime.UtcNow;
            Disabled = true;
            return this;
        }

        public TimeSpan GetDuration()
        {
            return (Exit ?? DateTime.UtcNow).Subtract(Entrance);
        }
    }
}
