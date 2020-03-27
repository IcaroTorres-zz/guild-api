using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class GuildDto
    {
        [Required]
        public string Name { get; set; }
        public Guid MasterId { get; set; }
        public HashSet<Guid> MemberIds { get; set; } = new HashSet<Guid>();
    }
}