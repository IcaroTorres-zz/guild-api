using Microsoft.AspNetCore.Mvc;
using System;

namespace Domain.DTOs
{
    public class MemberFilterDto
    {
        [FromQuery(Name = "name")] public string Name { get; set; } = string.Empty;
        [FromQuery(Name = "guildId")] public Guid GuildId { get; set; } = Guid.Empty;
        [FromQuery(Name = "count")] public int Count { get; set; } = 20;
    }
}
