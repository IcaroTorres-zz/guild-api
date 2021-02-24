using System;

namespace Application.Common.Responses
{
    [Serializable]
    public class MembershipResponse
    {
        public Guid Id { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public MemberGuildResponse Guild { get; set; }
        public GuildMemberResponse Member { get; set; }
    }
}
