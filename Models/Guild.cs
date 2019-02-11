using System;
using System.Collections.Generic;

namespace lumen.api.Models {
  public class Guild
  {
    public string Id { get; set; }
    public string MasterId { get; set; }
    public virtual User Master { get; set; }
    public virtual ICollection<User> Members { get; set; }
  }
}