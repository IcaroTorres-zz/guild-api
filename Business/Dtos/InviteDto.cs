using Domain.Enums;
using System;

namespace Business.Dtos
{
    [Serializable]
    public class InviteDto
    {
        public Guid Id { get; set; }
        public InviteStatuses Status { get; set; }
        public MemberGuildDto Guild { get; set; }
        public GuildMemberDto Member { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
