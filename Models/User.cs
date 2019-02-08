using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lumen.api.Models {
  public class User
  {
    public User ()
    {
      Gold = 0.0f;
      Level = 1;
      CreationDate = DateTime.Now;
    }
    [Key]
    public string Name { get; set; }
    public string GuildName { get; set; }
    public int Level { get; set; }
    public Double Gold { get; set; }
    public virtual Guild Guild { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsGuildMaster { get => Name.Equals(Guild?.MasterName); }
    public string FormatDate { get => CreationDate.ToString("d"); }
  }
}