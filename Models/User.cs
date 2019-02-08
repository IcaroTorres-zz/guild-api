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
    public int Level { get; set; }
    public Double Gold { get; set; }
    public DateTime CreationDate { get; set; }
    public string GuildName { get; set; }
    public bool IsGuildMaster { get; set; }
    public string FormatDate { get => CreationDate.ToString("d"); }
  }
  
  class UserEqualityComparer : IEqualityComparer<User>
  {
      public bool Equals(User u1, User u2)
      {
          if (u2 == null && u1 == null)
            return true;
          else if (u1 == null || u2 == null)
            return false;
          else if(u1.Name.Equals(u2.Name, StringComparison.OrdinalIgnoreCase))
              return true;
          else
              return false;
      }

      public int GetHashCode(User u) => u.Name.GetHashCode();
  }
}
