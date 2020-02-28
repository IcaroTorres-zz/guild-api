using JetBrains.Annotations;
using Newtonsoft.Json;
using System;

namespace Entities
{
    public class User : BaseEntity
    {
        protected User() { }

        public User(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; } = new Guid();
        public string Name { get; private set; }
        public Guid GuildId { get; private set; }
        public bool IsGuildMaster { get; private set; } = false;
        public virtual Guild Guild { get; private set; }

        [JsonIgnore]
        public virtual Membership Membership { get; private set; }

        public User ChangeName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            return this;
        }

        public User PromoteToGuildMaster()
        {
            IsGuildMaster = true;

            return this;
        }

        public User AcceptGuildInvitation([NotNull] Guild invitingGuild)
        {
            if (Guild != invitingGuild)
            {
                QuitGuild();
                GuildId = invitingGuild.Id;
                Guild = invitingGuild;
            }
            return this;
        }

        public User QuitGuild()
        {
            Guild?.KickMember(this);
            Guild = null;
            return this;
        }
    }
}
