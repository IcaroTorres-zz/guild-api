using Domain.Enums;
using System;

namespace Application.Common.Responses
{
    [Serializable]
    public class InviteResponse
    {
        public Guid Id { get; set; }
        public InviteStatuses Status { get; set; }
        public MemberGuildResponse Guild { get; set; }
        public GuildMemberResponse Member { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
