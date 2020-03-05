using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class GuildDto
    {
        public string Name { get; set; }
        public Guid MasterId { get; set; }
        public HashSet<Guid> MemberIds { get; set; } = new HashSet<Guid>();
    }
}