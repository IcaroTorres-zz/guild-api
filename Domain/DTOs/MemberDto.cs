using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class MemberDto
    {
        [Required] public string Name { get; set; }
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
    }
}
