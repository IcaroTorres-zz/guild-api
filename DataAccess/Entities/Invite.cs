using Domain.Enums;
using Newtonsoft.Json;
using System;

namespace DataAccess.Entities
{
    [Serializable]
    public class Invite : EntityModel<Invite>
    {
        public virtual InviteStatuses Status { get; set; }
        public Guid GuildId { get; set; }
        public Guid MemberId { get; set; }
        [JsonIgnore] public virtual Member Member { get; set; }
        [JsonIgnore] public virtual Guild Guild { get; set; }
    }
}
