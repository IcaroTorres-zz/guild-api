using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    [Serializable]
    public class Guild : EntityModel<Guild>
    {
        public virtual string Name { get; set; }
        [JsonIgnore] public virtual ICollection<Member> Members { get; set; } = new List<Member>();
        [JsonIgnore] public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();
    }
}