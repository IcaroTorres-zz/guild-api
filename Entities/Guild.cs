using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Guild : Entity<Guid>
    {
        public Guild(string name, User master)
        {
            Name = name;
            Master = master;
        }
        public Guild(string name, Guid masterId)
        {
            Name = name;
            MasterId = masterId;
        }

        public string Name { get; private set; }
        public Guid MasterId { get; private set; }
        public virtual User Master { get; private set; }

        [InverseProperty("Guild")]
        public virtual ICollection<User> Members { get; set; }

        public Guild ChangeName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            return this;
        }
        public Guild SetMaster(User newMaster)
        {
            Master = newMaster ?? throw new ArgumentNullException(nameof(newMaster));
            if (!Members.Contains(Master))
                Members.Add(Master);

            return this;
        }
        public Guild RemoveMember(User member)
        {
            if(member.Id == MasterId && Members.Count != 1)
                throw new InvalidOperationException($"Member '{member.Name}' is the Guildmaster of target '{Name}' guild. " +
                                                    $"Guildmasters can only leave guilds as the last remaining member. " +
                                                    $"(You can try PATCH 'api/guilds/{Id}' to transfer guild ownership).");
            Members.Remove(member);
            return this;
        }
        public Guild AddMember(User member)
        {
            if (Members.Contains(member))
                throw new DuplicateWaitObjectException(nameof(member), $"Member '{member.Name}' already in target '{Name}' guild.");

            Members.Add(member);
            return this;
        }
    }
}