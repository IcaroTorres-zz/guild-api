using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models {
  public class Guild : IEntity<string>
  {
    public string Id { get; set; }
    public string MasterId { get; set; }
    public virtual User Master { get; set; }

    [InverseProperty("Guild")]
    public virtual ICollection<User> Members { get; set; }
  }
}