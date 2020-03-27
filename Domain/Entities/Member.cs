using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    [Serializable]
    public class Member : EntityModel<Member>
    {
        public virtual string Name { get; set; }
        public virtual bool IsGuildMaster { get; set; }
        public virtual Guid? GuildId { get; set; }
        [JsonIgnore] public virtual Guild Guild { get; set; }
        [JsonIgnore] public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
