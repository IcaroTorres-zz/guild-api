using Domain.Enums;
using System;

namespace Persistence.Entities
{
    public class Invite : Domain.Models.Invite
    {
        public override InviteStatuses Status { get; protected set; }
        public override Guid? MemberId { get; protected set; }
        public override Guid? GuildId { get; protected set; }
        public override Domain.Models.Guild Guild { get; protected set; }
        public override Domain.Models.Member Member { get; protected set; }
    }
}