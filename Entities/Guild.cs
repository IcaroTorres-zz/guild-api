using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guild.Entities
{
    public class Guild : Entity<Guid>
    {
        public string Name { get; set; }
        public Guid MasterId { get; set; }
        public virtual User Master { get; set; }

        [InverseProperty("Guild")]
        public virtual ICollection<User> Members { get; set; }
    }
}