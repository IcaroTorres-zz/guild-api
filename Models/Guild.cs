using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lumen.api.Models {
  public class Guild
  {
    [Key]
    public string Name { get; set; }
    public string MasterName { get; set; }
    public List<string> Members { get; set; }
  }
}