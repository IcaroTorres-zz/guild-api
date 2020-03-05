using System;
using Microsoft.AspNetCore.Mvc;

namespace Domain.DTOs
{
    public class MemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GuildId { get; set; }
    }
}
