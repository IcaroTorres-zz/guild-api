using System;
using System.Collections.Generic;

namespace DTOs
{
    public class GuildDto
    {
        public string Name { get; set; }
        public string MasterName { get; set; }
        public HashSet<Guid> MemberIds { get; set; }
    }
}