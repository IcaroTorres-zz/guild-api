using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models {
  public class User : IEntity<string>
  {
    public string Id { get; set; }
    public string GuildId { get; set; }
    public virtual Guild Guild { get; set; }
    public bool IsGuildMaster { get => Guild?.MasterId?.Equals(Id) ?? false; }
  }
}
