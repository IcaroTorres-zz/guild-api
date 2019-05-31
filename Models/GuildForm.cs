using System;
using System.Collections.Generic;

namespace api.Models {
  public class GuildForm
  {
    public string Name { get; set; }
    public string MasterName { get; set; }
    public List<string> Members { get; set; }
  }
}