using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models {
  public class User
  {
    [Key]
    public string Name { get; set; }
    public string GuildName { get; set; }

    [ForeignKey("GuildName")]
    public virtual Guild Guild { get; set; }
    public bool IsGuildMaster { get => Guild?.MasterName?.Equals(Name) ?? false; }
  }
}
