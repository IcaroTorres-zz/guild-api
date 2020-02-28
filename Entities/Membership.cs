using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Membership
    {
        protected Membership() { }

        public Membership(Guild guild, User member)
        {
            Guild = guild;
            Member = member;
        }

        public DateTime Entrance { get; private set; } = DateTime.UtcNow;
        public DateTime? Exit { get; private set; }
        public bool Disabled { get; set; } = false;

        [ForeignKey("GuildId")]
        public virtual Guild Guild { get; set; }
        public Guid GuildId { get; private set; }

        [ForeignKey("MemberId")]
        public virtual User Member { get; set; }
        public Guid MemberId { get; private set; }

        public Membership RegisterExit()
        {
            Exit = DateTime.UtcNow;
            Disabled = true;
            return this;
        }
    }
}
