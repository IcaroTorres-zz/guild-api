using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs {
  public class GuildDto
  {
    [Required]
    public string Id { get; set; }
    [Required]
    public string MasterId { get; set; }
    public HashSet<string> Members { get; set; }
  }
}