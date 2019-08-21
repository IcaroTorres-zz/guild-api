using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User : Entity<Guid>
    {
        public User(string name) => Name = name;
        public string Name { get; private set; }
        public Guid GuildId { get; set; }
        public virtual Guild Guild { get; set; }
        [NotMapped]
        public bool IsGuildMaster => Guild?.MasterId == Id;
        public User ChangeName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            return this;
        }
    }
}
