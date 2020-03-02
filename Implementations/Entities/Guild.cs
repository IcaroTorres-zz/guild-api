using Abstractions.Entities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Implementations.Entities
{
    [Serializable]
    public class Guild : BaseEntity, IGuild
    {
        protected Guild() { }

        public Guild([NotNull] string name, Member master)
        {
            Id = Guid.NewGuid();
            Name = name;
            Promote(master);
            Validate();
        }

        public Guid Id { get; set; }
        public string Name { get; protected set; }
        public virtual ICollection<Member> Members { get; protected set; } = new List<Member>();

        public void ChangeName([NotNull] string name)
        {
            Name = name;
            Validate();
        }

        public void Invite([NotNull] IMember member)
        {
            if (member is Member memberToInvite)
            {
                if (!Members.Contains(memberToInvite))
                {
                    memberToInvite.AcceptInvitation(this);
                    Members.Add(memberToInvite);

                    if (Members.Count == 1)
                    {
                        memberToInvite.BePromoted();
                    }
                }
            }
        }

        public void Promote([NotNull] IMember member)
        {
            if (member is Member memberToPromote)
            {
                Invite(memberToPromote);
                memberToPromote.BePromoted();
            }
        }

        public void Demote(IMember previousMaster)
        {
            var memberToPromote = Members
                .OrderByDescending(m => m.Memberships
                    .SingleOrDefault(ms => !ms.Disabled)
                    .GetDuration())
                .FirstOrDefault(m => !m.IsGuildMaster);
            previousMaster.BeDemoted();
            memberToPromote?.BePromoted();
        }

        public void KickMember([NotNull] IMember member)
        {
            if (member is Member memberToKick)
            {
                if (Members.Contains(memberToKick))
                {
                    if (memberToKick.IsGuildMaster)
                    {
                        Demote(memberToKick);
                    }
                    Members.Remove(memberToKick);
                }
            }
        }

        public void UpdateMembers(IEnumerable<IMember> members)
        {
            // invite all incomming members
            foreach (var invited in members)
                Invite(invited);

            // kick previous not matching
            foreach (var kicked in Members.Except(members))
                KickMember(kicked);
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentNullException(nameof(Name));

            return true;
        }
    }
}