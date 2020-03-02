using Abstractions.Entities;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Implementations.Entities
{
    [Serializable]
    public class Member : BaseEntity, IMember
    {
        protected Member() { }

        public Member([NotNull] string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Memberships = new List<Membership>();
            Validate();
        }

        public Guid Id { get; set; }
        public string Name { get; protected set; }
        public Guid GuildId { get; set; }
        public bool IsGuildMaster { get; protected set; } = false;

        public virtual Guild Guild { get; protected set; }

        [JsonIgnore]
        public virtual ICollection<Membership> Memberships { get; protected set; }

        public void ChangeName([NotNull] string newName)
        {
            Name = newName;
            Validate();
        }

        public void AcceptInvitation([NotNull] IGuild guild)
        {
            if (guild is Guild invitingGuild && Guild != invitingGuild)
            {
                QuitGuild();
                Guild = invitingGuild;
                GuildId = Guild.Id;
                Memberships.Add(new Membership(Guild, this));
            }
        }

        public void BePromoted()
        {
            if (!IsGuildMaster)
            {
                IsGuildMaster = true;
                Guild?.Promote(this);
            }
        }

        public void BeDemoted()
        {
            IsGuildMaster = false;
        }

        public void QuitGuild()
        {
            Memberships
                .OrderBy(ms => ms.Entrance)
                .LastOrDefault()
                ?.RegisterExit();

            Guild?.KickMember(this);
            Guild = null;
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentNullException(nameof(Name));

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Member member && member.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Id.GetHashCode();
        }
    }
}
