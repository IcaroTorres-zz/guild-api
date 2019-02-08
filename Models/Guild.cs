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
}