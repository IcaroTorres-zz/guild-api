using System;

namespace Guild.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GuildId { get; set; }
        public string GuildName { get; set; }
    }
}
