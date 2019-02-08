using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lumen.api.Models {
  public class Guild
  {
    public Guild ()
    {
      Level = 1;
      CreationDate = DateTime.Now;
    }
    [Key]
    public string Name { get; set; }
    public string MasterName { get; set; }
    public int Level { get; set; }
    public virtual User Master { get; set; }
    public virtual ICollection<User> Members { get; set; }
    public DateTime CreationDate { get; set; }
  }
  class GuildEqualityComparer : IEqualityComparer<Guild>
  {
      public bool Equals(Guild g1, Guild g2)
      {
          if (g2 == null && g1 == null)
            return true;
          else if (g1 == null || g2 == null)
            return false;
          else if(g1.Name.Equals(g2.Name, StringComparison.OrdinalIgnoreCase))
              return true;
          else
              return false;
      }

      public int GetHashCode(Guild g) => g.Name.GetHashCode();
  }
}