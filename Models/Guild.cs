using System;
using System.Collections.Generic;

namespace lumen.api.Models {
  public class Guild
  {
    public string GuildName { get; set; }
    public string MasterName { get; set; }
    public int Level { get; set; }
    public virtual User Master { get; set; }
    public virtual ICollection<User> Members { get; set; }
    public DateTime CreationDate { get; set; }
  }
}