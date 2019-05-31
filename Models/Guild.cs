using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models {
  public class Guild
  {
    [Key]
    public string Name { get; set; }
    public string MasterName { get; set; }
    
    [ForeignKey("MasterName")]
    public virtual User Master { get; set; }

    [InverseProperty("Guild")]
    public virtual ICollection<User> Members { get; set; }
  }
}