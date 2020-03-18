using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Serializable]
    public class Membership : BaseEntity, IMembership
    {
        protected Membership() { }

        public Membership(Guild guild, Member member)
        {
            Id = Guid.NewGuid();
            Guild = guild;
            GuildId = guild.Id;
            Member = member;
            MemberId = member.Id;
        }
        public DateTime Entrance { get; protected set; } = DateTime.UtcNow;
        public DateTime? Exit { get; protected set; }

        [JsonIgnore, NotMapped] public override DateTime CreatedDate { get => Entrance; protected set {} }
        [JsonIgnore, NotMapped] public override DateTime ModifiedDate { get => Exit.Value; protected set {} }

        public virtual Member Member { get; protected set; }
        public Guid MemberId { get; protected set; }

        public virtual Guild Guild { get; protected set; }
        public Guid GuildId { get; protected set; }

        public IMembership RegisterExit()
        {
            Exit = DateTime.UtcNow;
            return this;
        }

        public TimeSpan GetDuration()
        {
            return (Exit ?? DateTime.UtcNow).Subtract(Entrance);
        }
    }
}
