using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Serializable]
    public partial class Membership : EntityModel<Membership>
    {
        public Membership() { }
        public virtual DateTime Since { get; internal set; } = DateTime.UtcNow;
        public virtual DateTime? Until { get; internal set; }
        public Guid MemberId { get; internal set; }
        public Guid GuildId { get; internal set; }
        [JsonIgnore] public virtual Guild Guild { get; internal set; }
        [JsonIgnore] public virtual Member Member { get; internal set; }
        [NotMapped] public override DateTime CreatedDate { get; internal set; } = DateTime.UtcNow;
        [NotMapped] public override DateTime ModifiedDate { get; internal set; } = DateTime.UtcNow;
    }
}
