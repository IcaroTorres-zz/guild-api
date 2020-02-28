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
        protected Guild() { }

        public Guild(string name, User master)
        {
            Name = name;
            PromoteToGuildMaster(master);
        }

        public Guid Id { get; private set; } = new Guid();
        public string Name { get; private set; }


        [InverseProperty("Guild")]
        public virtual ICollection<User> Members { get; private set; } = new List<User>();

        [JsonIgnore]
        public virtual ICollection<Membership> Memberships { get; private set; } = new List<Membership>();

        [NotMapped]
        public virtual User Master => Members.SingleOrDefault(m => m.IsGuildMaster);

        public Guild ChangeName(string newName)
        {
            Name = newName ?? Name;
            return this;
        }

        public Guild InviteToGuild([NotNull] User newMember)
        {
            if (!Members.Contains(newMember))
            {
                Members.Add(newMember.AcceptGuildInvitation(this));

                Memberships.Add(new Membership(this, newMember));

                if (Members.Count == 1)
                {
                    newMember.PromoteToGuildMaster();
                }
            }

            return this;
        }

        public Guild PromoteToGuildMaster([NotNull] User newMaster)
        {
            InviteToGuild(newMaster);
            newMaster.PromoteToGuildMaster();

            return this;
        }

        public Guild DemoteMaster()
        {
            var oldestMemberToReplaceMaster = GetOldestMemberToPromoteMaster();

            oldestMemberToReplaceMaster?.PromoteToGuildMaster();

            return this;
        }

        private User GetOldestMemberToPromoteMaster()
        {
            return Memberships.Join(Members,
                membership => membership.MemberId,
                member => member.Id,
                (membership, _) => membership)
                .OrderByDescending(ms => (ms.Exit ?? DateTime.UtcNow).Subtract(ms.Entrance))
                .FirstOrDefault(ms => !ms.Member.IsGuildMaster)?.Member;
        }

        public Guild KickMember([NotNull] User member)
        {
            if (Members.Contains(member))
            {
                if (Master == member)
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