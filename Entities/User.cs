using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guild.Entities
{
    public class User : Entity<Guid>
    {
        public string Name { get; set; }
        public Guid GuildId { get; set; }
        public virtual Guild Guild { get; set; }
        [NotMapped]
        public bool IsGuildMaster => Guild?.MasterId.Equals(Id) ?? false;
    }
}
