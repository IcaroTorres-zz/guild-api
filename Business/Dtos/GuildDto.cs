using System;
using System.Collections.Generic;

namespace Business.Dtos
{
    [Serializable]
    public class GuildDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GuildMemberDto Leader { get; set; }
        public List<GuildMemberDto> Members { get; set; } = new List<GuildMemberDto>();
        public int MembersCount => Members.Count;
    }

    [Serializable]
    public class GuildMemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGuildLeader { get; set; }
    }
}