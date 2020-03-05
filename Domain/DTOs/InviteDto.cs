using Microsoft.AspNetCore.Mvc;
using System;

namespace Domain.DTOs
{
    public class InviteDto
    {
        [FromQuery(Name = "guildId")] public Guid GuildId { get; set; } = Guid.Empty;
        [FromQuery(Name = "memberId")] public Guid MemberId { get; set; } = Guid.Empty;
        [FromQuery(Name = "count")] public int Count { get; set; } = 20;
    }
}
