using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lumen.api.Models {
  public class User
  {
    [Key]
    public string Name { get; set; }
    public string GuildName { get; set; }
    public bool IsGuildMaster { get; set; }
  }
}
