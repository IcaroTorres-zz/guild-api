using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataAccess.Entities
{
    [Serializable]
    public class Membership : EntityModel<Membership>
    {
        public virtual DateTime Entrance { get; set; } = DateTime.UtcNow;
        public virtual DateTime? Exit { get; set; }
        public Guid MemberId { get; set; }
        public Guid GuildId { get; set; }
        [JsonIgnore] public virtual Guild Guild { get; set; }
        [JsonIgnore] public virtual Member Member { get; set; }
        [NotMapped] public override DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        [NotMapped] public override DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;
    }
}
