using Newtonsoft.Json;
using System;

namespace DataAccess.Entities
{
    [Serializable]
    public class Invite : EntityModel<Invite>
    {
        public virtual InviteStatuses Status { get; set; } = InviteStatuses.Pending;
        public Guid GuildId { get; set; }
        public Guid MemberId { get; set; }
        [JsonIgnore] public virtual Member Member { get; set; }
        [JsonIgnore] public virtual Guild Guild { get; set; }
    }
    public enum InviteStatuses : short
    {
        /// <summary>
        /// Waiting for an answer as accepted, declined or canceled
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Accepted by invited member
        /// </summary>
        Accepted,
        /// <summary>
        /// Declined by invited member
        /// </summary>
        Declined,
        /// <summary>
        /// Canceled by inviting guild
        /// </summary>
        Canceled
    }
}
