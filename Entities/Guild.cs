using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class Guild : BaseEntity
    {
        private Guild() { }

        public Guild(string name, User master)
        {
            Name = name;
            PromoteToGuildMaster(master);
        }

        public Guid Id { get; private set; } = new Guid();
        public string Name { get; private set; }
        public Guid MasterId { get; private set; }
        public virtual User Master { get; private set; }

        [InverseProperty("Guild")]
        public virtual ICollection<User> Members { get; private set; }

        [JsonIgnore]
        public virtual ICollection<Membership> Memberships { get; private set; }

        public Guild ChangeName(string newName)
        {
            Name = newName ?? Name;
            return this;
        }

        public Guild PromoteToGuildMaster([NotNull] User newMaster)
        {
            InviteToGuild(newMaster);
            Master = Members.SingleOrDefault(m => m.Equals(newMaster)) ?? Master;

            return this;
        }

        public Guild DemoteMaster()
        {
            Master = GetOldestMemberToPromoteMaster();

            return this;
        }

        private User GetOldestMemberToPromoteMaster()
        {
            return Memberships.Join(Members, 
                membership => membership.MemberId, 
                member => member.Id, 
                (membership, _) => membership)
                .OrderByDescending(mb => (mb.Exit ?? DateTime.UtcNow).Subtract(mb.Entrance))
                .FirstOrDefault(m => !m.Equals(Master))?.Member;
        }

        public Guild InviteToGuild([NotNull] User newMember)
        {
            if (!Members.Contains(newMember))
            {
                Members.Add(newMember.AcceptGuildInvitation(this));

                Memberships.Add(new Membership(this, newMember));

                if (Members.Count == 1)
                {
                    Master = newMember;
                }
            }

            return this;
        }

        public Guild KickMember([NotNull] User member)
        {
            if (Members.Contains(member))
            {
                if (Master.Equals(member))
                {
                    DemoteMaster();
                }
                Members.Remove(member);
                member.Membership.RegisterExit();
            }

            return this;
        }
    }
}