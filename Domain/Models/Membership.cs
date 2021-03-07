using Domain.Common;
using Domain.Nulls;
using Domain.States.Memberships;
using System;

namespace Domain.Models
{
    public class Membership : EntityModel<Membership>
    {
        public static readonly NullMembership Null = new NullMembership();

        internal Membership ChangeState(MembershipState newState)
        {
            state = newState;
            ModifiedDate = newState.ModifiedDate;
            return this;
        }

        public virtual TimeSpan GetDuration()
        {
            return (ModifiedDate ?? DateTime.UtcNow).Subtract(CreatedDate);
        }

        public virtual Guid? MemberId { get; protected internal set; }
        public virtual Guid? GuildId { get; protected internal set; }
        public virtual string GuildName { get; protected internal set; }
        public virtual string MemberName { get; protected internal set; }

        protected MembershipState state;
        internal virtual MembershipState GetState()
        {
            state ??= MembershipState.NewState(this);
            return state;
        }
    }
}