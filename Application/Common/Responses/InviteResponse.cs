using Domain.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Responses
{
    [Serializable, ExcludeFromCodeCoverage]
    public class InviteResponse
    {
        public Guid Id { get; set; }
        public InviteStatuses Status { get; set; }
        public MemberGuildResponse Guild { get; set; }
        public GuildMemberResponse Member { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
