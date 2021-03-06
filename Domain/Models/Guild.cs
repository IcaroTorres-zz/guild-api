using Domain.Common;
using Domain.Nulls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Guild : EntityModel<Guild>
    {
        public static readonly NullGuild Null = new NullGuild();

        public virtual Guild ChangeName(string newName)
        {
            Name = newName;
            return this;
        }

        public virtual Invite InviteMember(Member member, IModelFactory factory)
        {
            if (!_members.Contains(member) && !(member is INullObject))
            {
                var invite = factory.CreateInvite(this, member);
                _invites.Add(invite);
                return invite;
            }
            return Invite.Null;
        }

        public virtual Membership RemoveMember(Member member)
        {
            return _members.Remove(member) ? member.State.Leave() : Membership.Null;
        }

        internal virtual Member AddMember(Member newMember)
        {
            return !(newMember is INullObject) && _members.Add(newMember) ? newMember : Member.Null;
        }

        public virtual Member Promote(Member member)
        {
            if (_members.Contains(member) && member.Guild == this)
            {
                GetLeader().State.BeDemoted();
                member.State.BePromoted();
                return member;
            }
            return Member.Null;
        }

        public virtual Member DemoteLeader()
        {
            return Members.Count > 1 ? Promote(GetVice()) : Member.Null;
        }

        public virtual string Name { get; protected internal set; }

        public HashSet<Member> _members = new HashSet<Member>();
        public virtual IReadOnlyCollection<Member> Members => _members;

        public List<Invite> _invites = new List<Invite>();
        public virtual IReadOnlyCollection<Invite> Invites => _invites;

        private static bool FilterGuildLeader(Member x) => x.IsGuildLeader;
        public Member GetLeader()
        {
            var leader = Members.SingleOrDefault(FilterGuildLeader);
            return leader ?? Member.Null;
        }

        private static TimeSpan OrderActiveMembershipByDuration(Member x) => x.GetActiveMembership().GetDuration();
        public Member GetVice()
        {
            var candidates = Members.Where(x => !FilterGuildLeader(x));
            candidates = candidates.OrderByDescending(OrderActiveMembershipByDuration);
            return candidates.FirstOrDefault() ?? Member.Null;
        }
        private static DateTime OrderInviteByCreation(Invite x) => x.CreatedDate;
        public Invite GetLatestInvite()
        {
            var orderedInvites = Invites.OrderByDescending(OrderInviteByCreation);
            return orderedInvites.FirstOrDefault() ?? Invite.Null;
        }
    }
}