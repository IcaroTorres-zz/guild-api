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
            if (!(member is INullObject) && !Members.Contains(member))
            {
                var invite = factory.CreateInvite(this, member);
                Invites.Add(invite);
                return invite;
            }
            return Invite.Null;
        }

        public virtual Member RemoveMember(Member member)
        {
            return !(member is INullObject) && Members.Remove(member) ? member.State.Leave() : Member.Null;
        }

        internal virtual Member AddMember(Member newMember)
        {
            return !(newMember is INullObject) && Members.Add(newMember) ? newMember : Member.Null;
        }

        public virtual Member Promote(Member member)
        {
            if (Members.FirstOrDefault(x => x.Id == member.Id) is { } guildMemberToPromote)
            {
                GetLeader().State.BeDemoted();
                guildMemberToPromote.State.BePromoted();
                return guildMemberToPromote;
            }

            return Member.Null;
        }

        public virtual Member DemoteLeader()
        {
            return Members.Count > 1 ? Promote(GetVice()) : Member.Null;
        }

        public virtual string Name { get; protected internal set; }

        public virtual HashSet<Member> Members { get; protected set; } = new HashSet<Member>();
        public virtual HashSet<Invite> Invites { get; protected set; } = new HashSet<Invite>();

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