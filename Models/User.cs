using System;
using System.Collections.Generic;

namespace lumen.api.Models {
  public class User
  {
    public string Id { get; set; }
    public string GuildId { get; set; }
    public Guild Guild { get; set; }
    public bool? IsGuildMaster { get => Guild?.MasterId.Equals(Id); }
  }
}
