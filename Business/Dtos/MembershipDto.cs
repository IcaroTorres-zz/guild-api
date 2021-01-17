using System;

namespace Business.Dtos
{
    [Serializable]
    public class MembershipDto
    {
        public Guid Id { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public MemberGuildDto Guild { get; set; }
        public GuildMemberDto Member { get; set; }
    }
}
