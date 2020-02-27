using System;
using System.Collections.Generic;

namespace DTOs
{
    public class GuildDto
    {
        public Guid Id { get; set; }
        public Guid MasterId { get; set; }
        public string Name { get; set; }
        public string MasterName { get; set; }
        public HashSet<UserDto> Members { get; set; }
    }
}