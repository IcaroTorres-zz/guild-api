using Newtonsoft.Json;
using System;

namespace Domain.Entities
{
  [Serializable]
  public partial class Invite : EntityModel<Invite>
  {
    public Invite() { }
    public virtual InviteStatuses Status { get; internal set; } = InviteStatuses.Pending;
    public Guid GuildId { get; internal set; }
    public Guid MemberId { get; internal set; }
    [JsonIgnore] public virtual Member Member { get; internal set; }
    [JsonIgnore] public virtual Guild Guild { get; internal set; }
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
