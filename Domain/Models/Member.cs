using Domain.Common;
using Domain.Nulls;
using Domain.States.Members;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Member : EntityModel<Member>
    {
        public static readonly NullMember Null = new NullMember();

        internal virtual Member ChangeState(MemberState newState)
        {
            state = newState;
            IsGuildLeader = newState.IsGuildLeader;
            if (newState.Guild is INullObject)
            {
                guild = null;
                GuildId = null;
            }
            else
            {
                guild = newState.Guild;
                GuildId = newState.Guild.Id;
            }

            return this;
        }

        public virtual Member ChangeName(string newName)
        {
            Name = newName;
            return this;
        }

        internal virtual Membership ActivateMembership(Guild guild, IModelFactory factory)
        {
            if (!(guild is INullObject))
            {
                var membership = factory.CreateMembership(guild, this);
                memberships.Add(membership);
                return membership;
            }
            return Membership.Null;
        }

        public virtual string Name { get; protected internal set; }
        public virtual bool IsGuildLeader { get; protected set; }
        public virtual Guid? GuildId { get; protected set; }

        private Guild guild;
        public virtual Guild GetGuild() => guild ?? Guild.Null;

        protected MemberState state;
        internal virtual MemberState GetState()
        {
            state ??= MemberState.NewState(this);
            return state;
        }

        public HashSet<Membership> memberships = new HashSet<Membership>();
        public virtual IReadOnlyCollection<Membership> Memberships => memberships;

        private static bool activeMembershipFilter(Membership x) => x.ModifiedDate == null;
        public Membership GetActiveMembership()
        {
            var activeMembership = Memberships.SingleOrDefault(activeMembershipFilter);
            return activeMembership ?? Membership.Null;
        }

        public Membership GetLastFinishedMembership()
        {
            var closeds = Memberships.Where(x => !activeMembershipFilter(x));
            closeds = closeds.OrderBy(x => x.ModifiedDate);
            var recentlyClosed = closeds.LastOrDefault();
            return recentlyClosed ?? Membership.Null;
        }
    }
}