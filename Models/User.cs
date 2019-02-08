using System;
using System.Collections.Generic;

namespace lumen.api.Models {
  public class User
  {
    public string UserName { get; set; }
    public string GuildName { get; set; }
    public int Level { get; set; }
    public Double Gold { get; set; }
    public virtual Guild Guild { get; set; }
    public virtual ICollection<User> Friends { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsGuildMaster { get => UserName.Equals(Guild?.MasterName); }
    public string FormatDate { get => CreateDate.ToString("d"); }
  }
}