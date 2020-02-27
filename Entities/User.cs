using JetBrains.Annotations;
using Newtonsoft.Json;
using System;

namespace Entities
{
    public class User : BaseEntity
    {
        private User() { }

        public User(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; } = new Guid();
        public string Name { get; private set; }
        public Guid GuildId { get; private set; }
        public virtual Guild Guild { get; private set; }

        [JsonIgnore]
        public virtual Membership Membership { get; private set; }

        public bool IsGuildMaster => Guild?.MasterId == Id;

        public User ChangeName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            return this;
        }

        public User QuitGuild()
        {
            Guild?.KickMember(this);
            Guild = null;
            return this;
        }

        public User PromoteToGuildMaster()
        {
            Guild?.PromoteToGuildMaster(this);
            return this;
        }

        public User AcceptGuildInvitation([NotNull] Guild invitingGuild)
        {
            if (!Guild.Equals(invitingGuild))
            {
                QuitGuild();
                Guild = invitingGuild;
            }
            return this;
        }
    }
}
